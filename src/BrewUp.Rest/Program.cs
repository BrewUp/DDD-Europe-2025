using BrewUp.Infrastructure.TextBasedDb;
using BrewUp.Persistence;
using BrewUp.Persistence.Sales.Queries;
using BrewUp.Persistence.Sales.Services;
using BrewUp.Persistence.Services;
using BrewUp.Persistence.Warehouses.Queries;
using BrewUp.Persistence.Warehouses.Services;
using BrewUp.Rest.Services;
using BrewUp.Rest.Validators.Warehouses;
using BrewUp.Shared.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Serilog;
using static BrewUp.Rest.Services.SalesOrderService;
using SalesOrderService = BrewUp.Persistence.Services.SalesOrderService;

var builder = WebApplication.CreateBuilder(args);

// Register Modules
builder.Services.AddCors(options => { options.AddPolicy("CorsPolicy", corsBuilder => corsBuilder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader()); });
var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
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
builder.Services.AddKeyedSingleton<IRepository, SaleRepository>("sale");
builder.Services.AddKeyedSingleton<IRepository, WarehouseRepository>("warehouse");

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddSingleton<ISalesOrderService, SalesOrderService>();
builder.Services.AddSingleton<ISalesQueryService, SalesQueryService>();
builder.Services.AddSingleton<IQueries<SalesOrder>, SalesOrderQueries>();

builder.Services.AddValidatorsFromAssemblyContaining<SetAvailabilityValidator>();
builder.Services.AddSingleton<ValidationHandler>();
builder.Services.AddSingleton<IWarehouseService, WarehouseService>();
builder.Services.AddSingleton<IAvailabilityQueryService, AvailabilityQueryService>();
builder.Services.AddSingleton<IQueries<Availability>, AvailabilityQueries>();

var app = builder.Build();

app.UseCors("CorsPolicy");

//Sales
var salesGroup = app.MapGroup("/v1/sales/").WithTags("Sales");

var salesOrderService = app.Services.GetRequiredService<ISalesOrderService>();

var handleCreateSalesOrder = HandleCreateSalesOrder(salesOrderService);

salesGroup.MapPost("/", handleCreateSalesOrder)
	.Produces(StatusCodes.Status400BadRequest)
	.Produces(StatusCodes.Status201Created)
	.WithName("CreateSalesOrder");

salesGroup.MapGet("/", HandleGetOrders)
	.Produces(StatusCodes.Status404NotFound)
	.Produces(StatusCodes.Status200OK)
	.WithName("GetSalesOrders");

//Warehouses
var warehousesGroup = app.MapGroup("/v1/warehouses/").WithTags("Warehouses");
warehousesGroup.MapPost("/availabilities", WarehousesService.HandleSetAvailabilities)
	.Produces(StatusCodes.Status400BadRequest)
	.Produces(StatusCodes.Status200OK)
	.WithName("SetAvailabilities");

// Configure the HTTP request pipeline.
app.UseSwagger(s =>
{
	s.RouteTemplate = "documentation/{documentName}/documentation.json";
});
app.UseSwaggerUI(s =>
{
	s.SwaggerEndpoint("/documentation/v1/documentation.json", "BrewUp");
	s.RoutePrefix = "documentation";
});

await app.RunAsync();
