using System.Linq.Expressions;
using BrewUp.Shared.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BrewUp.Persistence.Warehouses.Queries;

public sealed class AvailabilityQueries(IMongoClient mongoClient) : IQueries<Availability>
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase("Sales");

    public async Task<Availability> GetByIdAsync(string id)
    {
        var collection = _database.GetCollection<Availability>(nameof(Availability));
        var filter = Builders<Availability>.Filter.Eq("_id", id);
        return (await collection.CountDocumentsAsync(filter) > 0
            ? (await collection.FindAsync(filter).ConfigureAwait(false)).First()
            : null)!;
    }

    public async Task<PagedResult<Availability>> GetByFilterAsync(Expression<Func<Availability, bool>>? query, int page, int pageSize)
    {
        if (--page < 0)
            page = 0;

        var collection = _database.GetCollection<Availability>(nameof(Availability));
        var queryable = query != null
            ? collection.AsQueryable().Where(query)
            : collection.AsQueryable();

        var count = await queryable.CountAsync();
        var results = await queryable.Skip(page * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<Availability>(results, page, pageSize, count);
    }
}
