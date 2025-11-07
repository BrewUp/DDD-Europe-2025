using BrewUp.Shared.CustomTypes;

namespace BrewUp.Persistence.Entities.Sales;

public class SalesOrderRow
{
    internal readonly BeerId BeerId = default!;
    internal readonly BeerName BeerName = default!;

    internal readonly Quantity Quantity = default!;
    internal readonly Price BeerPrice = default!;

    protected SalesOrderRow()
    {
    }

    internal static SalesOrderRow CreateSalesOrderRow(BeerId beerId, BeerName beerName, Quantity quantity,
        Price price)
    {
        return new SalesOrderRow(beerId, beerName, quantity, price);
    }

    private SalesOrderRow(BeerId beerId, BeerName beerName, Quantity quantity, Price price)
    {
        BeerId = beerId;
        BeerName = beerName;
        Quantity = quantity;
        BeerPrice = price;
    }
}