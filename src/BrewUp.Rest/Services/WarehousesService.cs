using BrewUp.Persistence.Services;
using BrewUp.Shared.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BrewUp.Rest.Services;

public static class WarehousesService
{
    public static async Task<Ok> HandleSetAvailabilities(SetAvailabilityJson body, IWarehouseService warehousesDomainService)
    {
        await warehousesDomainService.UpdateAvailabilityDueToProductionOrderAsync(new Guid(body.BeerId), body.BeerName, body.Quantity);
        return TypedResults.Ok();
    }
}
