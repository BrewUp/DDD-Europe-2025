using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.DomainModel;
using Microsoft.Extensions.Logging;
using Temporalio.Activities;

namespace BrewUp.Mediator.WorkflowActivities;

public class SalesOrderActivities
{
    [Activity]
    public Task<string> CreateOrderAsync(SalesOrderJson salesOrder)
    {
        SalesOrderJson order = salesOrder with { SalesOrderId = Guid.NewGuid().ToString("N") };
        return Task.FromResult(order.SalesOrderId);
    }
    
    [Activity]
    public Task<SalesOrderJson> PrepareOrderAsync(SalesOrderJson salesOrder)
    {
        SalesOrderJson order = salesOrder with { SalesOrderId = Guid.NewGuid().ToString("N") };
        return Task.FromResult(order);
    }
    
    [Activity]
    public Task<decimal> ChargeCustomerCreditCard(SalesOrderJson salesOrder)
    {
        // Asking the customer to pay 50 EUR
        return Task.FromResult(50.0M);
    }
    
    [Activity]
    public Task<SalesOrderJson> ShipOrderAsync(SalesOrderJson salesOrder)
    {
        var logger = ActivityExecutionContext.Current.Logger;
        logger.LogInformation("ShipOrder invoked");
        
        SalesOrderJson orderToShip = salesOrder with { State = "Closed" };
        
        logger.LogInformation("ShipOrder complete");
        return Task.FromResult(orderToShip);
    }
    
    [Activity]
    public Task<PagedResult<BeerAvailabilityJson>> GetBeerAvailabilityAsync(SalesOrderRowJson row)
    {
        var logger = ActivityExecutionContext.Current.Logger;
        logger.LogInformation("GetBeerAvailability invoked");
        
        var result = new PagedResult<BeerAvailabilityJson>(new List<BeerAvailabilityJson>
        {
            new (row.BeerId.ToString(),row.BeerName, new Availability(row.Quantity.Value, row.Quantity.Value * 2, row.Quantity.UnitOfMeasure))
        }, 1, 10, 10);
        
        logger.LogInformation("GetBeerAvailability complete");
        return Task.FromResult(result);
    }
}