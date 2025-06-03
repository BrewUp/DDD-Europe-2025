using BrewUp.Mediator.Workflows;
using BrewUp.Sales.Facade;
using BrewUp.Shared.Contracts;
using BrewUp.Warehouses.Facade;
using Microsoft.Extensions.Logging;
using Temporalio.Client;

namespace BrewUp.Mediator;

public class BrewUpMediator(ISalesFacade salesFacade, 
	IWarehousesFacade warehouseFacade) : IBrewUpMediator
{
	public async Task<string> CreateOrderAsync(SalesOrderJson body, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		
		// Create a client to localhost on "default" namespace
		TemporalClient temporalClient = await TemporalClient.ConnectAsync(new TemporalClientConnectOptions("localhost:7233")
		{
			LoggerFactory = LoggerFactory.Create(builder =>
				builder.
					AddSimpleConsole(options => options.TimestampFormat = "[HH:mm:ss] ").
					SetMinimumLevel(LogLevel.Information)),
		});

		SalesOrderJson salesOrder = body with { SalesOrderId = body.SalesOrderId };
		var result = await temporalClient.ExecuteWorkflowAsync(
			(SalesOrderWorkflow wf) => wf.RunAsync(salesOrder),
			new(id: $"brewup-workflow-{GetSalesOrderNumber()}", taskQueue: "brewup-tasks"));		

		return result.OrderId;
	}

	private string GetSalesOrderNumber()
	{
		return
			$"{DateTime.Now.Year:0000}{DateTime.Now.Month:00}{DateTime.Now.Day:00}-{DateTime.Now.Hour:00}{DateTime.Now.Minute:00}{DateTime.Now.Second:00}";
	}
}