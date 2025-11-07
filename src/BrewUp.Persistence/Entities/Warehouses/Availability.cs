using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.Entities;

namespace BrewUp.Persistence.Entities.Warehouses;

public class Availability : AggregateRoot
{
    internal Guid BeerId = default!;
    internal BeerName BeerName = default!;
    internal Quantity Quantity = default!;

    protected Availability()
    {
    }

    internal static Availability CreateAvailability(Guid beerId, BeerName beerName, Quantity quantity)
    {
        return new Availability(beerId, beerName, quantity);
    }

    private Availability(Guid beerId, BeerName beerName, Quantity quantity)
    {
        Id = beerId.ToString();

        BeerId = beerId;
        BeerName = beerName;
        Quantity = quantity;
    }
}
