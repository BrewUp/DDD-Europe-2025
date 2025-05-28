using BrewUp.Mediator.WorkflowActivities;
using BrewUp.Mediator.WorkflowModels;
using BrewUp.Sales.Facade;
using BrewUp.Shared.Contracts;
using BrewUp.Warehouses.Facade;
using Temporalio.Exceptions;
using Temporalio.Workflows;

namespace BrewUp.Mediator;

[Workflow]
public class SalesOrderWorkflow
{
    [WorkflowRun]
    public async Task<OrderConfirmation> RunAsync(SalesOrderJson salesOrder,
        ISalesFacade salesFacade, IWarehousesFacade warehouseFacade,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var options = new ActivityOptions
        {
            StartToCloseTimeout = TimeSpan.FromSeconds(5),
            RetryPolicy = new() { MaximumInterval = TimeSpan.FromSeconds(10) },
        };
        
        List<BeerAvailabilityJson> availabilities = new();

        foreach (var row in salesOrder.Rows)
        {
            var availability = await Workflow.ExecuteActivityAsync(
                (SalesOrderActivities activity) => activity.GetBeerAvailabilityAsync(warehouseFacade, row.BeerId, cancellationToken),
                options);
            
            if (availability.TotalRecords > 0)
                availabilities.Add(availability.Results.First());
        }
        
        // Prepare the list of rows that are available for sale
        List<SalesOrderRowJson> rowsForSale = (from row in salesOrder.Rows
            let beerAvailability = availabilities.Find(a => a.BeerId == row.BeerId.ToString())
            where beerAvailability != null && beerAvailability.Availability.Available >= row.Quantity.Value
            select row).ToList();

        if (rowsForSale.Count == 0)
            throw new ApplicationFailureException("No beer available for sale");
        
        salesOrder = salesOrder with { Rows = rowsForSale };
        var orderId = await Workflow.ExecuteActivityAsync((SalesOrderActivities activity) => activity.CreateOrderAsync(salesFacade, salesOrder, cancellationToken), options);

        return new OrderConfirmation(
            orderId,
            OrderNumber: salesOrder.SalesOrderNumber,
            Status: "Crated",
            BillingTimestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Amount: salesOrder.Rows.Sum(r => r.Price.Value * r.Quantity.Value));
    }
}