using System.Linq.Expressions;
using BrewUp.Shared.Entities;

namespace BrewUp.Persistence.Warehouses;

public sealed class AvailabilityQueries : IQueries<Shared.Entities.Availability>
{
    public async Task<Shared.Entities.Availability> GetByIdAsync(string id)
    {
        string filePath = $"{IRepository.DbRoot}/warehouse-entity-{id}.json";
        if (!File.Exists(filePath))
        {
            return null;
        }

        string jsonString = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
        return jsonString.Deserialized<Shared.Entities.Availability>();
    }

    public async Task<PagedResult<Shared.Entities.Availability>> GetByFilterAsync(Expression<Func<Shared.Entities.Availability, bool>>? query, int page, int pageSize)
    {
        if (--page < 0)
            page = 0;

        var files = Directory.GetFiles(IRepository.DbRoot, "warehouse-entity-*.json");

        List<Task<Shared.Entities.Availability>> allOrdersT = files.Select(async filePath =>
        {
            string json = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
            var order = json.Deserialized<Shared.Entities.Availability>();
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

        return new PagedResult<Shared.Entities.Availability>(results, page, pageSize, count);
    }
}
