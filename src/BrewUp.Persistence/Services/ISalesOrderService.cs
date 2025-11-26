using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;

namespace BrewUp.Persistence.Services;

public delegate Task CreateSalesOrderStatic(
    SalesOrderId salesOrderId,
    SalesOrderNumber salesOrderNumber,
    OrderDate orderDate,
    CustomerId customerId,
    CustomerName customerName,
    IEnumerable<SalesOrderRowJson> rows);

public interface ISalesOrderService
{
    Task CreateSalesOrderAsync(SalesOrderId salesOrderId, SalesOrderNumber salesOrderNumber, OrderDate orderDate, CustomerId customerId,
        CustomerName customerName, IEnumerable<SalesOrderRowJson> rows);
}
