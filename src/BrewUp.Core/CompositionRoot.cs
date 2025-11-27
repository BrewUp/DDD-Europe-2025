using BrewUp.Core.Sales;
using BrewUp.Core.Warehouses;
using Serilog.Core;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.Entities;

namespace BrewUp.Core;

public delegate Task<PagedResult<SalesOrderJson>> GetSalesOrders(int page, int pageSize);

public delegate Task UpdateAvailabilityDueToProductionOrder(
    Guid beerId,
    string beerName,
    Quantity quantity);

public delegate Task CreateSalesOrderStatic(
    SalesOrderId salesOrderId,
    SalesOrderNumber salesOrderNumber,
    OrderDate orderDate,
    CustomerId customerId,
    CustomerName customerName,
    IEnumerable<SalesOrderRowJson> rows);


public record CompositionRoot(
    GetSalesOrders GetSalesOrders,
    CreateSalesOrderStatic CreateSalesOrder,
    UpdateAvailabilityDueToProductionOrder UpdateAvailabilityDueToProductionOrder)
{
    public static CompositionRoot Build(Logger logger) =>
        new(
            GetSalesOrders:
                SalesQueryService.GetSalesOrders(
                    logger,
                    SalesOrderQueries.GetSalesOrderByFilter),

            CreateSalesOrder:
                SalesOrderService.CreateSalesOrder(
                    SaleRepository.InsertSalesOrder,
                    WarehouseRepository.GetAvailabilityById),

            UpdateAvailabilityDueToProductionOrder:
                WarehouseService.UpdateAvailabilityDueToProductionOrder(
                    WarehouseRepository.InsertAvailability));
}
