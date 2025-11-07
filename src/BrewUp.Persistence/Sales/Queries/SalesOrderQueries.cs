using System.Linq.Expressions;
using BrewUp.Shared.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace BrewUp.Persistence.Sales.Queries;

public sealed class SalesOrderQueries(IMongoClient mongoClient) : IQueries<SalesOrder>
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase("Sales");

    public async Task<SalesOrder> GetByIdAsync(string id)
    {
        var collection = _database.GetCollection<SalesOrder>(nameof(SalesOrder));
        var filter = Builders<SalesOrder>.Filter.Eq("_id", id);
        return (await collection.CountDocumentsAsync(filter) > 0
            ? (await collection.FindAsync(filter).ConfigureAwait(false)).First()
            : null)!;
    }

    public async Task<PagedResult<SalesOrder>> GetByFilterAsync(Expression<Func<SalesOrder, bool>>? query, int page, int pageSize)
    {
        if (--page < 0)
            page = 0;

        var collection = _database.GetCollection<SalesOrder>(nameof(SalesOrder));
        var queryable = query != null
            ? collection.AsQueryable().Where(query)
            : collection.AsQueryable();

        var count = await queryable.CountAsync();
        var results = await queryable.Skip(page * pageSize).Take(pageSize).ToListAsync();

        return new PagedResult<SalesOrder>(results, page, pageSize, count);
    }
}
