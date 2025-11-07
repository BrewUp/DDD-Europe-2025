using BrewUp.Persistence.Sales.Services;
using BrewUp.Persistence.Services;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BrewUp.Rest.Services;

public static class SalesOrderService
{
	public static async Task<Results<Created, NotFound>> HandleCreateSalesOrder(ISalesOrderService salesOrderService, SalesOrderJson body, CancellationToken cancellationToken)
	{
		await salesOrderService.CreateSalesOrderAsync(new Guid(body.SalesOrderId),
			body.SalesOrderNumber, body.OrderDate,
			body.CustomerId, body.CustomerName,
			body.Rows, cancellationToken);

		return TypedResults.Created($"v1/sales/{body.SalesOrderId}");
	}

	public static async Task<Results<Ok<PagedResult<SalesOrderJson>>, NotFound>> HandleGetOrders(ISalesQueryService salesQueryService, CancellationToken cancellationToken)
	{
		var orders = await salesQueryService.GetSalesOrdersAsync(0, 30, cancellationToken);
		return TypedResults.Ok(orders);
	}
}
