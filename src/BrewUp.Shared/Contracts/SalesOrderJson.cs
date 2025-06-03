namespace BrewUp.Shared.Contracts;

public record SalesOrderJson(
    string SalesOrderId,
    string SalesOrderNumber,
    Guid CustomerId,
    string CustomerName,
    DateTime OrderDate,
    string State,
    IEnumerable<SalesOrderRowJson> Rows);
