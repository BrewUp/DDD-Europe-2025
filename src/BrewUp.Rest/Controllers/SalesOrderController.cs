using BrewUp.Persistence.Sales.Services;
using BrewUp.Persistence.Services;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BrewUp.Rest.Controllers;

[ApiController]
[Route("v1/sales")]
[Produces("application/json")]
[Tags("Sales")]
public class SalesOrderController
{
    private readonly ISalesOrderService _salesOrderService;
    private readonly ISalesQueryService _salesQueryService;

    public SalesOrderController(ISalesOrderService salesOrderService, ISalesQueryService salesQueryService)
    {
        _salesOrderService = salesOrderService;
        _salesQueryService = salesQueryService;
    }

    //    [HttpPost]
    //    [ProducesResponseType(StatusCodes.Status201Created)]
    //    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //    [ActionName("CreateSalesOrder")]
    // public async Task<Results<Created, NotFound>> HandleCreateSalesOrder(SalesOrderJson body)
    // {
    // 	await _salesOrderService.CreateSalesOrderAsync(new Guid(body.SalesOrderId),
    // 		body.SalesOrderNumber, body.OrderDate,
    // 		body.CustomerId, body.CustomerName,
    // 		body.Rows);
    //
    // 	return TypedResults.Created($"v1/sales/{body.SalesOrderId}");
    // }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ActionName("GetSalesOrders")]
    public async Task<Results<Ok<PagedResult<SalesOrderJson>>, NotFound>> HandleGetOrders()
    {
        var orders = await _salesQueryService.GetSalesOrdersAsync(0, 30);
        return TypedResults.Ok(orders);
    }
}

internal static class SalesOrderControllerStatic
{
    internal static async Task<Results<Created, NotFound>> HandleCreateSalesOrder(
        ISalesOrderService salesOrderService,
        SalesOrderJson body)
    {
        await salesOrderService.CreateSalesOrderAsync(new Guid(body.SalesOrderId),
            body.SalesOrderNumber, body.OrderDate,
            body.CustomerId, body.CustomerName,
            body.Rows);

        return TypedResults.Created($"v1/sales/{body.SalesOrderId}");
    }
}