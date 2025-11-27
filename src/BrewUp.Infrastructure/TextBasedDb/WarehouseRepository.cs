using BrewUp.Persistence;
using BrewUp.Persistence.Services;
using BrewUp.Shared.Entities;

namespace BrewUp.Infrastructure.TextBasedDb;

public class WarehouseRepository : IRepository
{
    public async Task<T> GetByIdAsync<T>(string id) where T : EntityBase
    {
        if (!File.Exists(WarehouseRepositoryStatic.FileName(id))) return await Task.FromResult<T>(null);

        return (await File.ReadAllTextAsync(WarehouseRepositoryStatic.FileName(id))).Deserialized<T>();
    }

    public async Task InsertAsync<T>(T entity) where T : EntityBase
    {
        await File.WriteAllTextAsync(WarehouseRepositoryStatic.FileName(entity.Id), entity.Serialized());
    }

    public async Task UpdateAsync<T>(T entity) where T : EntityBase
    {
        await File.WriteAllTextAsync(WarehouseRepositoryStatic.FileName(entity.Id), entity.Serialized());
    }

    public async Task DeleteAsync<T>(T entity) where T : EntityBase
    {
        await Task.Run(() => File.Delete(WarehouseRepositoryStatic.FileName(entity.Id)));
    }
}

public static class WarehouseRepositoryStatic
{
    internal static string FileName(string id) => $"{IRepository.DbRoot}/warehouse-entity-{id}.json";

    public static async Task<Availability> GetAvailabilityById(string id)
    {
        if (!File.Exists(FileName(id))) return await Task.FromResult<Availability>(null);

        return (await File.ReadAllTextAsync(FileName(id))).Deserialized<Availability>();
    }
}
