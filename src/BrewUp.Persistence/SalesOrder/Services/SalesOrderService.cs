using BrewUp.Persistence.Services;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.Entities;

namespace BrewUp.Persistence.SalesOrder.Services;

public delegate Task CreateSalesOrder(Guid salesOrderId, string salesOrderNumber, DateTime orderDate,
    Guid customerId, string customerName, IEnumerable<SalesOrderRowJson> rows);

public static class SalesOrderServiceStatic
{
    public static CreateSalesOrder CreateSalesOrder(
        IRepository saleRepository,
        IRepository warehouseRepository) =>
        async (salesOrderId, salesOrderNumber, orderDate, customerId, customerName, rows) =>
        {
            List<SalesOrderRowJson> beersAvailable = new();
            foreach (var row in rows)
            {
                var availability =
                    await warehouseRepository.GetByIdAsync<Availability>(row.BeerId.ToString());
                if (availability != null)
                    beersAvailable.Add(row);
            }

            var aggregate = Entities.Sales.SalesOrder.CreateSalesOrder(salesOrderId, salesOrderNumber, orderDate,
                customerId,
                customerName,
                beersAvailable);

            await saleRepository.InsertAsync(aggregate.MapToSharedDto());
        };
}