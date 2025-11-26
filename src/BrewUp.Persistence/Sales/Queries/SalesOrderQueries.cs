using System.Linq.Expressions;
using BrewUp.Persistence.Services;
using BrewUp.Shared.Entities;

namespace BrewUp.Persistence.Sales.Queries;

public sealed class SalesOrderQueries : IQueries<SalesOrder>
{
    public async Task<SalesOrder> GetByIdAsync(string id)
    {
        string filePath = $"{IRepository.DbRoot}/sales-entity-{id}.json";
        if (!File.Exists(filePath))
        {
            return null;
        }

        string jsonString = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
        return jsonString.Deserialized<SalesOrder>();
    }

    public async Task<PagedResult<SalesOrder>> GetByFilterAsync(Expression<Func<SalesOrder, bool>>? query, int page, int pageSize)
    {
        if (--page < 0)
            page = 0;

        var files = Directory.GetFiles(IRepository.DbRoot, "sales-entity-*.json");

        List<Task<SalesOrder>> allOrdersT = files.Select(async filePath =>
        {
            string json = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
            var order = json.Deserialized<SalesOrder>();
            return order;
        }).ToList();

        var allOrders = (await Task.WhenAll(allOrdersT).ConfigureAwait(false)).ToList();

        var queryable = allOrders.AsQueryable();

        if (query != null)
            queryable = queryable.Where(query);

        var count = queryable.Count();
        var results = queryable
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<SalesOrder>(results, page, pageSize, count);
    }
}
