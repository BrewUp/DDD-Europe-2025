using BrewUp.Shared.Contracts;

namespace BrewUp.Persistence.Services;

public interface ISalesOrderService
{
    Task CreateSalesOrderAsync(Guid salesOrderId, string salesOrderNumber, DateTime orderDate, Guid customerId,
        string customerName, IEnumerable<SalesOrderRowJson> rows);
}
