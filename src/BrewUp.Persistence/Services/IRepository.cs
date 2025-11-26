using BrewUp.Shared.Entities;

namespace BrewUp.Persistence.Services;

public interface IRepository
{
    public const string DbRoot = "db";

    Task<T> GetByIdAsync<T>(string id) where T : EntityBase;
    Task InsertAsync<T>(T entity) where T : EntityBase;
}
