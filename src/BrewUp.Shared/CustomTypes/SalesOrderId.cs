namespace BrewUp.Shared.CustomTypes;

public record SalesOrderId(
    Guid Value)
{
    public static SalesOrderId Of(string salesOrderId) =>
        new(
            new Guid(salesOrderId));

}
