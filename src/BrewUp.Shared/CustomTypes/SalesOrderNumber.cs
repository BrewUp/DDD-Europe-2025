namespace BrewUp.Shared.CustomTypes;

public record SalesOrderNumber(string Value)
{
    public static SalesOrderNumber Of(string salesOrderNumber) =>
        new(salesOrderNumber);
}
