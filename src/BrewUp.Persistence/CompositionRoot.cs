using BrewUp.Persistence.Sales;
using BrewUp.Persistence.Warehouses;
using Serilog.Core;

namespace BrewUp.Persistence;

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
