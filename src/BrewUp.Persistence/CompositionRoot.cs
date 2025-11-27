using BrewUp.Persistence.Sales;
using BrewUp.Persistence.Warehouses;
using Serilog.Core;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.Entities;

namespace BrewUp.Persistence;

public delegate Task<PagedResult<SalesOrderJson>> GetSalesOrders(int page, int pageSize);

public record CompositionRoot(
    GetSalesOrders GetSalesOrders,
    CreateSalesOrderStatic CreateSalesOrderStatic,
    UpdateAvailabilityDueToProductionOrder UpdateAvailabilityDueToProductionOrder)
{
    public static CompositionRoot Build(Logger logger) =>
        new(
            GetSalesOrders:
                SalesQueryService.GetSalesOrders(
                    logger,
                    SalesOrderQueries.GetSalesOrderByFilter),

            CreateSalesOrderStatic:
                SalesOrderService.CreateSalesOrder(
                    SaleRepositoryStatic.InsertSalesOrder,
                    WarehouseRepositoryStatic.GetAvailabilityById),

            UpdateAvailabilityDueToProductionOrder:
                WarehouseService.UpdateAvailabilityDueToProductionOrder(
                    WarehouseRepositoryStatic.InsertAvailability));
}
