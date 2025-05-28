using BrewUp.Sales.Facade;
using BrewUp.Shared.Contracts;
using BrewUp.Warehouses.Facade;
using Microsoft.Extensions.Logging;
using Temporalio.Client;

namespace BrewUp.Mediator;

public class BrewUpMediator(ISalesFacade salesFacade, IWarehousesFacade warehouseFacade) : IBrewUpMediator
{
	public async Task<string> CreateOrderAsync(SalesOrderJson body, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		TemporalClient temporalClient = await TemporalClient.ConnectAsync(new TemporalClientConnectOptions("localhost:7233")
		{
			LoggerFactory = LoggerFactory.Create(builder =>
				builder.
					AddSimpleConsole(options => options.TimestampFormat = "[HH:mm:ss] ").
					SetMinimumLevel(LogLevel.Information)),
		});
		
		var result = await temporalClient.ExecuteWorkflowAsync(
			(SalesOrderWorkflow wf) => wf.RunAsync(body, salesFacade, warehouseFacade, cancellationToken),
			new WorkflowOptions	
			{
				Id = $"sales-order-workflow-{body.SalesOrderNumber}",
				TaskQueue = "sales-order-task-queue",
			});

		return result.OrderId;
	}
}