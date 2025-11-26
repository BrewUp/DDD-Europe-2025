using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.Entities;

namespace BrewUp.Persistence.Entities.Warehouses;

public class Availability : AggregateRoot
{
    internal Guid BeerId = default!;
    internal string BeerName = default!;
    internal Quantity Quantity = default!;

    protected Availability()
    {
    }

    internal static Availability CreateAvailability(Guid beerId, string beerName, Quantity quantity)
    {
        return new Availability(beerId, beerName, quantity);
    }

    private Availability(Guid beerId, string beerName, Quantity quantity)
    {
        Id = beerId.ToString();

        BeerId = beerId;
        BeerName = beerName;
        Quantity = quantity;
    }
}
