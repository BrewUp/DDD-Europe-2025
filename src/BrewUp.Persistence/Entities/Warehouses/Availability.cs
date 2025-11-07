using BrewUp.Shared.CustomTypes;
using BrewUp.Shared.Entities;

namespace BrewUp.Persistence.Entities.Warehouses;

public class Availability : AggregateRoot
{
    internal BeerId BeerId = default!;
    internal BeerName BeerName = default!;
    internal Quantity Quantity = default!;

    protected Availability()
    {
    }

    internal static Availability CreateAvailability(BeerId beerId, BeerName beerName, Quantity quantity)
    {
        return new Availability(beerId, beerName, quantity);
    }

    private Availability(BeerId beerId, BeerName beerName, Quantity quantity)
    {
        Id = beerId.Value.ToString();

        BeerId = beerId;
        BeerName = beerName;
        Quantity = quantity;
    }
}