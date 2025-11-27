using BrewUp.Infrastructure.TextBasedDb;
using BrewUp.Rest;
using BrewUp.Rest.Validators.Warehouses;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Serilog;
using static BrewUp.Rest.Services.SalesOrderService;
using static BrewUp.Rest.Services.WarehousesService;

var builder = WebApplication.CreateBuilder(args);

// Register Modules
builder.Services.AddCors(options => { options.AddPolicy("CorsPolicy", corsBuilder => corsBuilder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader()); });
var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
builder.Logging.AddSerilog(logger);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup => setup.SwaggerDoc(
    "v1", new OpenApiInfo
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

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<SetAvailabilityValidator>();
builder.Services.AddSingleton<ValidationHandler>();

var app = builder.Build();

app.UseCors("CorsPolicy");

//Sales
var compositionRoot = CompositionRoot.Build(logger);

var salesGroup = app.MapGroup("/v1/sales/");
salesGroup.MapPost("/", HandleCreateSalesOrder(compositionRoot.CreateSalesOrderStatic));
salesGroup.MapGet("/", HandleGetOrders(compositionRoot.GetSalesOrders));

var warehousesGroup = app.MapGroup("/v1/warehouses/").WithTags("Warehouses");
warehousesGroup.MapPost("/availabilities", HandleSetAvailabilities(compositionRoot.UpdateAvailabilityDueToProductionOrder));

// Configure the HTTP request pipeline.
app.UseSwagger(s => { s.RouteTemplate = "documentation/{documentName}/documentation.json"; });
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/documentation/v1/documentation.json", "BrewUp");
    s.RoutePrefix = "documentation";
});

await app.RunAsync();
