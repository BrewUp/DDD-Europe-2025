using BrewUp.Shared.Contracts;

namespace BrewUp.Shared.Entities;

public class SalesOrder : EntityBase
{
    public string SalesOrderNumber { get; private set; } = new(string.Empty);
    public DateTime OrderDate { get; private set; } = DateTime.MinValue;

    public Guid CustomerId { get; private set; } = Guid.Empty;
    public string CustomerName { get; private set; } = string.Empty;

    public IEnumerable<SalesOrderRowJson> Rows { get; private set; } = Enumerable.Empty<SalesOrderRowJson>();

    protected SalesOrder()
    {
    }

    public static SalesOrder Create(Guid salesOrderId, string salesOrderNumber, DateTime orderDate, Guid customerId,
        string customerName, IEnumerable<SalesOrderRowJson> rows)
    {
        return new SalesOrder
        {
            Id = salesOrderId.ToString(),
            SalesOrderNumber = salesOrderNumber,
            OrderDate = orderDate,

            CustomerId = customerId,
            CustomerName = customerName,

            Rows = rows
        };
    }

    public SalesOrderJson ToJson() => new(Id, SalesOrderNumber, CustomerId, CustomerName, OrderDate, Rows);
}
