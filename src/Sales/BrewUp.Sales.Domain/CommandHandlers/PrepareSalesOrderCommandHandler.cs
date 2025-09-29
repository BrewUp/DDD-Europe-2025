using BrewUp.Sales.Domain.Entities;
using BrewUp.Sales.SharedKernel.Commands;
using Microsoft.Extensions.Logging;
using Muflone.Messages.Commands;
using Muflone.Persistence;

namespace BrewUp.Sales.Domain.CommandHandlers;

public sealed class PrepareSalesOrderCommandHandler(IRepository repository, ILoggerFactory loggerFactory)
    : CommandHandlerAsync<PrepareSalesOrder>(repository, loggerFactory)
{
    public override async Task HandleAsync(PrepareSalesOrder command, CancellationToken cancellationToken = new CancellationToken())
    {
        var aggregate = await Repository.GetByIdAsync<SalesOrder>(command.AggregateId, cancellationToken)
            .ConfigureAwait(false);
        aggregate!.PrepareSalesOrder();
        await Repository.SaveAsync(aggregate, Guid.NewGuid(), cancellationToken).ConfigureAwait(false);
    }
}