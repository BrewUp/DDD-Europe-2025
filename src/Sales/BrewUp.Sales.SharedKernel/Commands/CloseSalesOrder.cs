using BrewUp.Sales.SharedKernel.CustomTypes;
using Muflone.Messages.Commands;

namespace BrewUp.Sales.SharedKernel.Commands;

public class CloseSalesOrder(SalesOrderId aggregateId, Guid commitId) : 
    Command(aggregateId, commitId)
{
}