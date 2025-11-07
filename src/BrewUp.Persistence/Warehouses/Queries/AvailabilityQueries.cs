using System.Linq.Expressions;
using BrewUp.Persistence.Services;
using BrewUp.Shared.Entities;

namespace BrewUp.Persistence.Warehouses.Queries;

public sealed class AvailabilityQueries : IQueries<Availability>
{
    public async Task<Availability> GetByIdAsync(string id)
    {
        string filePath = $"{IRepository.DbRoot}/warehouse-entity-{id}.json";
        if (!File.Exists(filePath))
        {
            return null;
        }

        string jsonString = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
        return jsonString.Deserialized<Availability>();
    }

    public async Task<PagedResult<Availability>> GetByFilterAsync(Expression<Func<Availability, bool>>? query, int page, int pageSize)
    {
        if (--page < 0)
            page = 0;

        var files = Directory.GetFiles(IRepository.DbRoot, "warehouse-entity-*.json");

        List<Task<Availability>> allOrdersT = files.Select(async filePath =>
        {
            string json = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
            var order = json.Deserialized<Availability>();
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

        return new PagedResult<Availability>(results, page, pageSize, count);
    }
}
