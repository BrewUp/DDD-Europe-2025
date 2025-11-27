using BrewUp.Shared.CustomTypes;

namespace BrewUp.Core.Sales;

public class SalesOrderRow
{
    internal readonly Guid BeerId = default!;
    internal readonly string BeerName = default!;

    internal readonly Quantity Quantity = default!;
    internal readonly Price BeerPrice = default!;

    protected SalesOrderRow()
    {
    }

    internal static SalesOrderRow CreateSalesOrderRow(Guid beerId, string beerName, Quantity quantity,
        Price price)
    {
        return new SalesOrderRow(beerId, beerName, quantity, price);
    }

    private SalesOrderRow(Guid beerId, string beerName, Quantity quantity, Price price)
    {
        BeerId = beerId;
        BeerName = beerName;
        Quantity = quantity;
        BeerPrice = price;
    }
}
