using BrewUp.Sales.Facade;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.DomainModel;
using BrewUp.Warehouses.Facade;
using Temporalio.Activities;

namespace BrewUp.Mediator.WorkflowActivities;

public class SalesOrderActivities
{
    [Activity]
    public async Task<PagedResult<BeerAvailabilityJson>> GetBeerAvailabilityAsync(IWarehousesFacade warehousesFacade, Guid beerId, 
        CancellationToken cancellationToken)
    {
        return await warehousesFacade.GetAvailabilityAsync(beerId, cancellationToken);
    }

    [Activity]
    public async Task<string> CreateOrderAsync(ISalesFacade salesFacade, SalesOrderJson salesOrder,
        CancellationToken cancellationToken)
    {
        // This method should be implemented to create a sales order
        // using the sales facade.
        return await salesFacade.CreateOrderAsync(salesOrder, cancellationToken);
    }
}