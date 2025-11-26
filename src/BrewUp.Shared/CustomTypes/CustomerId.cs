namespace BrewUp.Shared.CustomTypes;

public record CustomerId(Guid Value)
{
    public static CustomerId Of(Guid customerId) => new(customerId);
}