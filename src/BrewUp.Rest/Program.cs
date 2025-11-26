using BrewUp.Infrastructure.TextBasedDb;
using BrewUp.Persistence;
using BrewUp.Persistence.SalesOrder.Queries;
using BrewUp.Persistence.SalesOrder.Services;
using BrewUp.Persistence.Services;
using BrewUp.Persistence.Warehouses.Queries;
using BrewUp.Persistence.Warehouses.Services;
using BrewUp.Rest.Validators.Warehouses;
using BrewUp.Shared.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Serilog;
using static BrewUp.Persistence.SalesOrder.Services.SalesOrderServiceStatic;
using static BrewUp.Rest.Controllers.SalesOrderControllerStatic;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Register Modules
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", corsBuilder => corsBuilder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());
});
var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.AddSerilog(logger);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup => setup.SwaggerDoc("v1", new OpenApiInfo()
{
    Description = "BrewUp",
    Title = "BrewUp API",
    Version = "v1",
    Contact = new OpenApiContact
    {
        Name = "BrewUp"
    }
}));

builder.Services.AddFileBasedDb();
builder.Services.AddKeyedSingleton<IRepository, WarehouseRepository>("warehouse");

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddSingleton<ISalesQueryService, SalesQueryService>();
builder.Services.AddSingleton<IQueries<SalesOrder>, SalesOrderQueries>();

builder.Services.AddValidatorsFromAssemblyContaining<SetAvailabilityValidator>();
builder.Services.AddSingleton<ValidationHandler>();
builder.Services.AddSingleton<IWarehouseService, WarehouseService>();
builder.Services.AddSingleton<IAvailabilityQueryService, AvailabilityQueryService>();
builder.Services.AddSingleton<IQueries<Availability>, AvailabilityQueries>();

var app = builder.Build();
app.MapControllers();

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
app.UseSwagger(s => { s.RouteTemplate = "documentation/{documentName}/documentation.json"; });
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/documentation/v1/documentation.json", "BrewUp");
    s.RoutePrefix = "documentation";
});

var saleRepository = new SaleRepository();
var warehouseRepository = new WarehouseRepository();

var createSalesOrderHandle = CreateSalesOrderHandle(
    CreateSalesOrder(
        saleRepository,
        warehouseRepository
    )
);

app.MapPost("v1/sales", createSalesOrderHandle);

app.MapGet("v1/sales", async (HttpContext context) =>
{
    var salesQueryService = context.RequestServices.GetRequiredService<ISalesQueryService>();
    return await HandleGetOrders(salesQueryService);
});

await app.RunAsync();