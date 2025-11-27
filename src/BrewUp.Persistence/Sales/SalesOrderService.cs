using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using Availability = BrewUp.Shared.Entities.Availability;

namespace BrewUp.Persistence.Sales;

public delegate Task CreateSalesOrderStatic(
    SalesOrderId salesOrderId,
    SalesOrderNumber salesOrderNumber,
    OrderDate orderDate,
    CustomerId customerId,
    CustomerName customerName,
    IEnumerable<SalesOrderRowJson> rows);

internal delegate Task InsertAsync(Shared.Entities.SalesOrder salesOrder);

internal delegate Task<Availability> GetWareHouse(string id);

internal static class SalesOrderService
{
    internal static CreateSalesOrderStatic CreateSalesOrder(
        InsertAsync insertSalesOrder,
        GetWareHouse getWareHouse) =>
        async (
            salesOrderId,
            salesOrderNumber,
            orderDate,
            customerId,
            customerName,
            rows) =>
        {
            List<SalesOrderRowJson> beersAvailable = new();
            foreach (var row in rows)
            {
                await getWareHouse(row.BeerId.ToString());
                beersAvailable.Add(row);
            }

            var aggregate = SalesOrder.CreateSalesOrder(salesOrderId, salesOrderNumber, orderDate, customerId, customerName, beersAvailable);

            await insertSalesOrder(aggregate.MapToSharedDto());
        };
}
