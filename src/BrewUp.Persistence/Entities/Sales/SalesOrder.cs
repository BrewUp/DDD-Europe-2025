using BrewUp.Shared.Contracts;
using BrewUp.Shared.Entities;

namespace BrewUp.Persistence.Entities.Sales;

public class SalesOrder : AggregateRoot
{
    internal readonly Guid SalesOrderId = default!;
    internal readonly string SalesOrderNumber = default!;
    internal readonly DateTime OrderDate = default!;

    internal readonly Guid CustomerId = default!;
    internal readonly string CustomerName = default!;

    internal readonly IEnumerable<SalesOrderRow> Rows = Enumerable.Empty<SalesOrderRow>();

    protected SalesOrder()
    {
    }

    internal static SalesOrder CreateSalesOrder(Guid salesOrderId, string salesOrderNumber,
        DateTime orderDate, Guid customerId, string customerName, IEnumerable<SalesOrderRowJson> rows)
    {
        return new SalesOrder(salesOrderId, salesOrderNumber, orderDate, customerId, customerName, rows.MapToDomainRows());
    }

    private SalesOrder(Guid salesOrderId, string salesOrderNumber, DateTime orderDate,
        Guid customerId, string customerName, IEnumerable<SalesOrderRow> row)
    {
        SalesOrderId = salesOrderId;
        SalesOrderNumber = salesOrderNumber;
        OrderDate = orderDate;

        CustomerId = customerId;
        CustomerName = customerName;

        Rows = row;
    }
}
