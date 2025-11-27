using BrewUp.Persistence;
using BrewUp.Shared.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BrewUp.Rest.Handlers;

public delegate Task<Ok> HandleSetAvailabilities(SetAvailabilityJson body);

internal static class WarehousesHandler
{
    internal static HandleSetAvailabilities HandleSetAvailabilities(UpdateAvailabilityDueToProductionOrder updateAvailabilityDueToProductionOrder) =>
        async body =>
        {
            await updateAvailabilityDueToProductionOrder(new Guid(body.BeerId), body.BeerName, body.Quantity);
            return TypedResults.Ok();
        };
}
