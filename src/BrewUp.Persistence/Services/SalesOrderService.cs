using BrewUp.Persistence.Entities.Sales;
using BrewUp.Shared.Contracts;
using Availability = BrewUp.Shared.Entities.Availability;
using BrewUp.Shared.CustomTypes;

namespace BrewUp.Persistence.Services;

public delegate Task CreateSalesOrderStatic(
    SalesOrderId salesOrderId,
    SalesOrderNumber salesOrderNumber,
    OrderDate orderDate,
    CustomerId customerId,
    CustomerName customerName,
    IEnumerable<SalesOrderRowJson> rows);

public delegate Task InsertAsync(Shared.Entities.SalesOrder salesOrder);
public delegate Task<Availability> GetWareHouse(string id);

public static class SalesOrderService
{
    public static CreateSalesOrderStatic CreateSalesOrder(
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
