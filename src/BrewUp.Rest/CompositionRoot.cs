using BrewUp.Persistence.Entities.Sales;
using BrewUp.Persistence.Entities.Warehouses;
using BrewUp.Persistence.Sales.Queries;
using BrewUp.Persistence.Sales.Services;
using BrewUp.Persistence.Services;
using Serilog.Core;

namespace BrewUp.Rest;

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
