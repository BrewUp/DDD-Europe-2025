using BrewUp.Sales.SharedKernel.CustomTypes;
using Muflone.Messages.Events;

namespace BrewUp.Sales.SharedKernel.Events;

public class SalesOrderPrepared(SalesOrderId aggregateId, Guid commitId) : 
    DomainEvent(aggregateId, commitId)
{
}