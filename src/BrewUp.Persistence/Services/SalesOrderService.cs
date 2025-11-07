using BrewUp.Persistence.Entities.Sales;
using BrewUp.Shared.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace BrewUp.Persistence.Services;

public sealed class SalesOrderService(
    [FromKeyedServices("sale")] IRepository saleRepository,
    [FromKeyedServices("warehouse")] IRepository warehouseRepository) : ISalesOrderService
{
    public async Task CreateSalesOrderAsync(Guid salesOrderId, string salesOrderNumber, DateTime orderDate,
        Guid customerId, string customerName, IEnumerable<SalesOrderRowJson> rows)
    {
        List<SalesOrderRowJson> beersAvailable = new();
        foreach (var row in rows)
        {
            var availability = await warehouseRepository.GetByIdAsync<Shared.Entities.Availability>(row.BeerId.ToString());
            if (availability != null)
                beersAvailable.Add(row);
        }

        var aggregate = SalesOrder.CreateSalesOrder(salesOrderId, salesOrderNumber, orderDate, customerId, customerName, beersAvailable);

        await saleRepository.InsertAsync(aggregate.MapToSharedDto());
    }
}
