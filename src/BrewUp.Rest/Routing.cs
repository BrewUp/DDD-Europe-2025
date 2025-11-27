using BrewUp.Persistence;
using BrewUp.Rest.Handlers;

namespace BrewUp.Rest;

internal static class Routing
{
    internal static void DefineRoutes(this WebApplication webApplication, CompositionRoot compositionRoot1)
    {
        var salesGroup = webApplication.MapGroup("/v1/sales/");
        salesGroup.MapPost("/", SalesOrderHandler.HandleCreateSalesOrder(compositionRoot1.CreateSalesOrderStatic));
        salesGroup.MapGet("/", SalesOrderHandler.HandleGetOrders(compositionRoot1.GetSalesOrders));

        var warehousesGroup = webApplication.MapGroup("/v1/warehouses/").WithTags("Warehouses");
        warehousesGroup.MapPost("/availabilities", WarehousesHandler.HandleSetAvailabilities(compositionRoot1.UpdateAvailabilityDueToProductionOrder));
    }
}
