using BrewUp.Persistence.Sales.Services;
using BrewUp.Persistence.Services;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BrewUp.Rest.Services;

public delegate Task<Results<Created, NotFound>> HandleCreateSalesOrder(SalesOrderJson body);

public delegate Task CreateSalesOrder(
    SalesOrderId salesOrderId,
    SalesOrderNumber salesOrderNumber,
    OrderDate orderDate,
    CustomerId customerId,
    CustomerName customerName,
    IEnumerable<SalesOrderRowJson> salesOrders);

public static class SalesOrderService
{
    public static HandleCreateSalesOrder HandleCreateSalesOrder(CreateSalesOrderStatic salesOrderStatic) =>
        async body =>
        {
            await salesOrderStatic(
                SalesOrderId.Of(body.SalesOrderId),
                SalesOrderNumber.Of(body.SalesOrderNumber),
                OrderDate.Of(body.OrderDate),
                CustomerId.Of(body.CustomerId),
                CustomerName.Of(body.CustomerName),
                body.Rows);

            return TypedResults.Created($"v1/sales/{body.SalesOrderId}");
        };


    public static async Task<Results<Ok<PagedResult<SalesOrderJson>>, NotFound>> HandleGetOrders(ISalesQueryService salesQueryService)
    {
        var orders = await salesQueryService.GetSalesOrdersAsync(0, 30);
        return TypedResults.Ok(orders);
    }
}
