using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.Entities;

namespace BrewUp.Persistence.Entities.Sales;

public class SalesOrder : AggregateRoot
{
    internal readonly SalesOrderId SalesOrderId = default!;
    internal readonly SalesOrderNumber SalesOrderNumber = default!;
    internal readonly OrderDate OrderDate = default!;

    internal readonly CustomerId CustomerId = default!;
    internal readonly CustomerName CustomerName = default!;

    internal readonly IEnumerable<SalesOrderRow> Rows = Enumerable.Empty<SalesOrderRow>();

    protected SalesOrder()
    {
    }

    internal static SalesOrder CreateSalesOrder(SalesOrderId salesOrderId, SalesOrderNumber salesOrderNumber,
        OrderDate orderDate, CustomerId customerId, CustomerName customerName, IEnumerable<SalesOrderRowJson> rows)
    {
        return new SalesOrder(salesOrderId, salesOrderNumber, orderDate, customerId, customerName, rows.MapToDomainRows());
    }

    private SalesOrder(SalesOrderId salesOrderId, SalesOrderNumber salesOrderNumber, OrderDate orderDate,
        CustomerId customerId, CustomerName customerName, IEnumerable<SalesOrderRow> row)
    {
        SalesOrderId = salesOrderId;
        SalesOrderNumber = salesOrderNumber;
        OrderDate = orderDate;

        CustomerId = customerId;
        CustomerName = customerName;

        Rows = row;
    }
}
