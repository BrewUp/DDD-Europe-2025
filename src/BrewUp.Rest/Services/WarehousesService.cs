using BrewUp.Persistence.Services;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BrewUp.Rest.Services;

public delegate Task<Ok> HandleSetAvailabilities(SetAvailabilityJson body);

public static class WarehousesService
{
    public static HandleSetAvailabilities HandleSetAvailabilities(UpdateAvailabilityDueToProductionOrder updateAvailabilityDueToProductionOrder) =>
        async body =>
        {
            await updateAvailabilityDueToProductionOrder(new Guid(body.BeerId), body.BeerName, body.Quantity);
            return TypedResults.Ok();
        };
}
