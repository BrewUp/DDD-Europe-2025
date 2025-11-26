namespace BrewUp.Shared.CustomTypes;

public record OrderDate(DateTime Value)
{
    public static OrderDate Of(DateTime date) => new(date);
}
