namespace BrewUp.Shared.CustomTypes;

public record CustomerName(string Value)
{
    public static CustomerName Of(string customerName) => new(customerName);
}