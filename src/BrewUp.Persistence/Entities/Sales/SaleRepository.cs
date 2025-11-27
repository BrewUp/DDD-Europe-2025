using BrewUp.Persistence.Services;
using BrewUp.Shared.Entities;

namespace BrewUp.Persistence.Entities.Sales;

public class SaleRepository : IRepository
{
    public async Task<T> GetByIdAsync<T>(string id) where T : EntityBase
    {
        if (!File.Exists(SaleRepositoryStatic.FileName(id))) return await Task.FromResult<T>(null);
        return (await File.ReadAllTextAsync($"entity-{id}")).Deserialized<T>();
    }

    public async Task InsertAsync<T>(T entity) where T : EntityBase
    {
        await File.WriteAllTextAsync(SaleRepositoryStatic.FileName(entity.Id), entity.Serialized());
    }

    public async Task UpdateAsync<T>(T entity) where T : EntityBase
    {
        await File.WriteAllTextAsync(SaleRepositoryStatic.FileName(entity.Id), entity.Serialized());
    }

    public async Task DeleteAsync<T>(T entity) where T : EntityBase
    {
        await Task.Run(() => File.Delete(SaleRepositoryStatic.FileName(entity.Id)));
    }
}

public static class SaleRepositoryStatic
{
    public static async Task InsertSalesOrder(Shared.Entities.SalesOrder salesOrder)
    {
        await File.WriteAllTextAsync(FileName(salesOrder.Id), salesOrder.Serialized());
    }

    internal static string FileName(string id) => $"{IRepository.DbRoot}/sales-entity-{id}.json";
}
