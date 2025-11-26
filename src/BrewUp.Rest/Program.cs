using BrewUp.Infrastructure.TextBasedDb;
using BrewUp.Persistence;
using BrewUp.Persistence.Sales.Queries;
using BrewUp.Persistence.Sales.Services;
using BrewUp.Persistence.Services;
using BrewUp.Persistence.Warehouses.Queries;
using BrewUp.Persistence.Warehouses.Services;
using BrewUp.Rest.Controllers;
using BrewUp.Rest.Validators.Warehouses;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;
using SalesOrderService = BrewUp.Persistence.Services.SalesOrderService;

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
builder.Services.AddKeyedScoped<IRepository, SaleRepository>("sale");
builder.Services.AddKeyedScoped<IRepository, WarehouseRepository>("warehouse");

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
builder.Services.AddScoped<ISalesQueryService, SalesQueryService>();
builder.Services.AddScoped<IQueries<SalesOrder>, SalesOrderQueries>();

builder.Services.AddValidatorsFromAssemblyContaining<SetAvailabilityValidator>();
builder.Services.AddSingleton<ValidationHandler>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IAvailabilityQueryService, AvailabilityQueryService>();
builder.Services.AddScoped<IQueries<Availability>, AvailabilityQueries>();

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


app.MapPost("v1/sales", (HttpContext context, [FromBody] SalesOrderJson body) =>
{
    ISalesOrderService salesOrderService = context.RequestServices.GetRequiredService<ISalesOrderService>();
    return SalesOrderControllerStatic.HandleCreateSalesOrder(salesOrderService, body);
});

app.MapGet("v1/sales", async (HttpContext context) =>
{
    var salesQueryService = context.RequestServices.GetRequiredService<ISalesQueryService>();
    return await SalesOrderControllerStatic.HandleGetOrders(salesQueryService);
});

await app.RunAsync();