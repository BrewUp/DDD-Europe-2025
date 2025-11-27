using BrewUp.Persistence;
using BrewUp.Rest;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => { options.AddPolicy("CorsPolicy", corsBuilder => corsBuilder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader()); });
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

var app = builder.Build();
app.UseCors("CorsPolicy");

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
var compositionRoot = CompositionRoot.Build(logger);
app.DefineRoutes(compositionRoot);


app.UseSwagger(s => { s.RouteTemplate = "documentation/{documentName}/documentation.json"; });
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/documentation/v1/documentation.json", "BrewUp");
    s.RoutePrefix = "documentation";
});

await app.RunAsync();
