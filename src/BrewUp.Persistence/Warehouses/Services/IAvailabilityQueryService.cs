using BrewUp.Shared.Contracts;
using BrewUp.Shared.Entities;

namespace BrewUp.Persistence.Warehouses.Services
{
	public interface IAvailabilityQueryService
	{
		Task<PagedResult<BeerAvailabilityJson>> GetAvailabilityAsync(Guid beerId, CancellationToken cancellationToken);
	}
}