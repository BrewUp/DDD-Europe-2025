using BrewUp.Mediator.WorkflowActivities;
using BrewUp.Mediator.WorkflowModels;
using BrewUp.Shared.Contracts;
using Temporalio.Exceptions;
using Temporalio.Workflows;

namespace BrewUp.Mediator.Workflows;

[Workflow]
public class SalesOrderWorkflow
{
    [WorkflowRun]
    public async Task<OrderConfirmation> RunAsync(SalesOrderJson salesOrder)
    {
        var options = new ActivityOptions
        {
            StartToCloseTimeout = TimeSpan.FromMinutes(3),
            RetryPolicy = new() { MaximumInterval = TimeSpan.FromSeconds(10) },
        };
        
        // List<BeerAvailabilityJson> availabilities = [];
        //
        // var order = salesOrder;
        // var availability = await Workflow.ExecuteActivityAsync(
        //     (SalesOrderActivities activity) => activity.GetBeerAvailabilityAsync(order.Rows.First()),
        //     options).ConfigureAwait(false);
        //     
        // if (availability.TotalRecords > 0)
        //     availabilities.Add(availability.Results.First());
        //
        // if (availability.TotalRecords == 0)
        // {
        //     // If no beer is available, we cannot proceed with the order
        //     throw new ApplicationFailureException("No beer available for the order.");
        // }
        
        // await Workflow.DelayAsync(TimeSpan.FromSeconds(10));
        
        // Ready to ship the order
        var shippedOrder = await Workflow.ExecuteActivityAsync(
                (SalesOrderActivities activity) =>
                    activity.ShipOrderAsync(salesOrder), options)
            .ConfigureAwait(false);

        return new OrderConfirmation(
            shippedOrder.SalesOrderId,
            OrderNumber: salesOrder.SalesOrderNumber,
            Status: shippedOrder.State,
            BillingTimestamp: DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Amount: salesOrder.Rows.Sum(r => r.Price.Value * r.Quantity.Value));
    }
}