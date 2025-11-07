using BrewUp.Persistence.Services;
using BrewUp.Shared.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BrewUp.Rest.Controllers;

[ApiController]
[Route("/v1/warehouses")]
[Produces("application/json")]
[Tags("Sales")]
public class WarehousesControllernew
{
    [HttpPost]
    [Route("availabilities")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ActionName("SetAvailabilities")]
    public async Task<Ok> HandleSetAvailabilities(SetAvailabilityJson body, IWarehouseService warehousesDomainService)
    {
        await warehousesDomainService.UpdateAvailabilityDueToProductionOrderAsync(new Guid(body.BeerId), body.BeerName, body.Quantity);
        return TypedResults.Ok();
    }
}
