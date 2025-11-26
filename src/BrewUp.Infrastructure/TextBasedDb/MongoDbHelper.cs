using BrewUp.Persistence.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BrewUp.Infrastructure.TextBasedDb;

public static class MongoDbHelper
{
	public static IServiceCollection AddFileBasedDb(this IServiceCollection services)
    {
        Directory.CreateDirectory(IRepository.DbRoot);

		return services;
	}
}
