using BrewUp.Persistence.Sales;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BrewUp.Rest.Services;

public delegate Task<Results<Created, NotFound>> HandleCreateSalesOrder(SalesOrderJson body);

public delegate Task<Results<Ok<PagedResult<SalesOrderJson>>, NotFound>> HandleGetOrders();

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


    public static HandleGetOrders HandleGetOrders(GetSalesOrders getGetSalesOrders) => async () =>
    {
        var orders = await getGetSalesOrders(0, 30);
        return TypedResults.Ok(orders);
    };
}
