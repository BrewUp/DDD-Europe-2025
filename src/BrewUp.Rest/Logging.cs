using Serilog;
using Serilog.Core;

namespace BrewUp.Rest;

static class Logging
{
    internal static Logger Build(ConfigurationManager configuration) =>
        new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();
}
