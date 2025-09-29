using BrewUp.Sales.SharedKernel.Commands;
using BrewUp.Sales.SharedKernel.CustomTypes;
using BrewUp.Sales.SharedKernel.Events;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Events;
using Muflone.Persistence;

namespace BrewUp.Sales.ReadModel.EventHandlers;

public class SalesOrderPreparedEventHandler(ILoggerFactory loggerFactory,
    IServiceBus servicebus)
    : DomainEventHandlerAsync<SalesOrderPrepared>(loggerFactory)
{
    public override async Task HandleAsync(SalesOrderPrepared @event, CancellationToken cancellationToken = new())
    {
        cancellationToken.ThrowIfCancellationRequested();

        CloseSalesOrder command = new((SalesOrderId) @event.AggregateId, @event.MessageId);
		
        await servicebus.SendAsync(command, cancellationToken).ConfigureAwait(false);
    }
}