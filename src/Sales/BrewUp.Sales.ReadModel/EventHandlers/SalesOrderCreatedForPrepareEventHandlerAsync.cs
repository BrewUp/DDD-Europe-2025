using BrewUp.Sales.SharedKernel.Commands;
using BrewUp.Sales.SharedKernel.CustomTypes;
using BrewUp.Sales.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using Muflone.Persistence;

namespace BrewUp.Sales.ReadModel.EventHandlers;

public sealed class SalesOrderCreatedForPrepareEventHandlerAsync(ILoggerFactory loggerFactory,
	IServiceBus servicebus)
	: DomainEventHandlerAsync<SalesOrderCreated>(loggerFactory)
{
	public override async Task HandleAsync(SalesOrderCreated @event, CancellationToken cancellationToken = new())
	{
		cancellationToken.ThrowIfCancellationRequested();

		PrepareSalesOrder command = new((SalesOrderId) @event.AggregateId, @event.MessageId);
		
		await servicebus.SendAsync(command, cancellationToken).ConfigureAwait(false);
	}
}