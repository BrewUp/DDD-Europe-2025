using BrewUp.Persistence.Entities.Sales;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using Microsoft.Extensions.DependencyInjection;
using Availability = BrewUp.Shared.Entities.Availability;

namespace BrewUp.Persistence.Services;

public sealed class SalesOrderService(
    [FromKeyedServices("sale")] IRepository saleRepository,
    [FromKeyedServices("warehouse")] IRepository warehouseRepository) : ISalesOrderService
{
    public async Task CreateSalesOrderAsync(
        SalesOrderId salesOrderId,
        SalesOrderNumber salesOrderNumber,
        OrderDate orderDate,
        CustomerId customerId,
        CustomerName customerName,
        IEnumerable<SalesOrderRowJson> rows)
    {
        await CreateSalesOrder
            (saleRepository.InsertAsync, warehouseRepository.GetByIdAsync<Availability>)
            (salesOrderId, salesOrderNumber, orderDate, customerId, customerName, rows);
    }

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

public delegate Task InsertAsync(Shared.Entities.SalesOrder salesOrder);
public delegate Task<Availability> GetWareHouse(string id);
