using System.Linq.Expressions;
using BrewUp.Shared.Entities;

namespace BrewUp.Persistence;

public interface IQueries<T> where T : EntityBase
{
    Task<T> GetByIdAsync(string id);
    Task<PagedResult<T>> GetByFilterAsync(Expression<Func<T, bool>>? query, int page, int pageSize);
}
