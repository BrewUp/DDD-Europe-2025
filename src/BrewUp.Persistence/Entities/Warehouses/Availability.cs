using BrewUp.Persistence.Services;
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

    public static CreateAvailability CreateAvailability =
        (beerId, beerName, quantity) => new Availability(beerId, beerName, quantity);

    private Availability(Guid beerId, string beerName, Quantity quantity)
    {
        Id = beerId.ToString();

        BeerId = beerId;
        BeerName = beerName;
        Quantity = quantity;
    }
}
