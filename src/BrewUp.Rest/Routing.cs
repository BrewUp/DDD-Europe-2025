using BrewUp.Core;
using BrewUp.Rest.Handlers;

namespace BrewUp.Rest;

internal static class Routing
{
    internal static void DefineRoutes(this WebApplication webApplication, CompositionRoot compositionRoot)
    {
        var salesGroup = webApplication.MapGroup("/v1/sales/");
        salesGroup.MapPost("/", SalesOrderHandler.HandleCreateSalesOrder(compositionRoot.CreateSalesOrder));
        salesGroup.MapGet("/", SalesOrderHandler.HandleGetOrders(compositionRoot.GetSalesOrders));

        var warehousesGroup = webApplication.MapGroup("/v1/warehouses/").WithTags("Warehouses");
        warehousesGroup.MapPost("/availabilities", WarehousesHandler.HandleSetAvailabilities(compositionRoot.UpdateAvailabilityDueToProductionOrder));
    }
}
