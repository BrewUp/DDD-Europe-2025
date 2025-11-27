using BrewUp.Rest.Services;

namespace BrewUp.Rest;

internal static class Routing
{
    internal static void DefineRoutes(this WebApplication webApplication, CompositionRoot compositionRoot1)
    {
        var salesGroup = webApplication.MapGroup("/v1/sales/");
        salesGroup.MapPost("/", SalesOrderService.HandleCreateSalesOrder(compositionRoot1.CreateSalesOrderStatic));
        salesGroup.MapGet("/", SalesOrderService.HandleGetOrders(compositionRoot1.GetSalesOrders));

        var warehousesGroup = webApplication.MapGroup("/v1/warehouses/").WithTags("Warehouses");
        warehousesGroup.MapPost("/availabilities", WarehousesService.HandleSetAvailabilities(compositionRoot1.UpdateAvailabilityDueToProductionOrder));
    }
}
