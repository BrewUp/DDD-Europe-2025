using Microsoft.Extensions.Logging;

namespace BrewUp.Persistence
{
    public abstract class ServiceBase
    {
        protected readonly ILogger Logger;

        protected ServiceBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }
    }
}