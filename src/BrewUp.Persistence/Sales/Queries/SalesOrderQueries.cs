using System.Linq.Expressions;
using BrewUp.Persistence.Services;
using BrewUp.Shared.Entities;

namespace BrewUp.Persistence.Sales.Queries;

public delegate Task<PagedResult<SalesOrder>> GetSalesOrderByFilter(
    Expression<Func<SalesOrder, bool>>? query,
    int page,
    int pageSize);


public static class SalesOrderQueries
{
    public static GetSalesOrderByFilter GetSalesOrderByFilter =
        async (query, page, pageSize) =>
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
    };
}
