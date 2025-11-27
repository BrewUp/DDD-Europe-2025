using BrewUp.Rest;
using BrewUp.Rest.Validators.Warehouses;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Serilog;

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

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssemblyContaining<SetAvailabilityValidator>();
builder.Services.AddSingleton<ValidationHandler>();

var app = builder.Build();
app.UseCors("CorsPolicy");

var compositionRoot = CompositionRoot.Build(logger);
app.DefineRoutes(compositionRoot);


app.UseSwagger(s => { s.RouteTemplate = "documentation/{documentName}/documentation.json"; });
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/documentation/v1/documentation.json", "BrewUp");
    s.RoutePrefix = "documentation";
});

await app.RunAsync();
