using BrewUp.Persistence;
using BrewUp.Persistence.Services;
using BrewUp.Shared.Entities;

namespace BrewUp.Infrastructure.TextBasedDb;

public class WarehouseRepository : IRepository
{
    private static string FileName(string id) => $"{IRepository.DbRoot}/warehouse-entity-{id}.json";

    public async Task<T> GetByIdAsync<T>(string id) where T : EntityBase
    {
        if (!File.Exists(FileName(id))) return await Task.FromResult<T>(null);

        return (await File.ReadAllTextAsync(FileName(id))).Deserialized<T>();
    }

    public async Task InsertAsync<T>(T entity) where T : EntityBase
    {
        await File.WriteAllTextAsync(FileName(entity.Id), entity.Serialized());
    }

    public async Task UpdateAsync<T>(T entity) where T : EntityBase
    {
        await File.WriteAllTextAsync(FileName(entity.Id), entity.Serialized());
    }

    public async Task DeleteAsync<T>(T entity) where T : EntityBase
    {
        await Task.Run(() => File.Delete(FileName(entity.Id)));
    }
}
