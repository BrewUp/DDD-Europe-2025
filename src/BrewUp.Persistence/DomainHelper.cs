using BrewUp.Persistence.Entities.Sales;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;

namespace BrewUp.Persistence;

public static class DomainHelper
{
    internal static SalesOrderRow MapToDomainRow(this SalesOrderRowJson json)
    {
        return SalesOrderRow.CreateSalesOrderRow(new BeerId(json.BeerId), new BeerName(json.BeerName), json.Quantity, json.Price);
    }

    internal static IEnumerable<SalesOrderRow> MapToDomainRows(this IEnumerable<SalesOrderRowJson> json)
    {
        return json.Select(r => SalesOrderRow.CreateSalesOrderRow(new BeerId(r.BeerId), new BeerName(r.BeerName), r.Quantity, r.Price));
    }

    internal static Shared.Entities.SalesOrder MapToSharedDto(this Entities.Sales.SalesOrder salesOrder)
    {
        return Shared.Entities.SalesOrder.Create(salesOrder.SalesOrderId, salesOrder.SalesOrderNumber,
            salesOrder.OrderDate, salesOrder.CustomerId, salesOrder.CustomerName,
            salesOrder.Rows.Select(r => new SalesOrderRowJson
            {
                BeerId = r.BeerId.Value,
                BeerName = r.BeerName.Value,
                Quantity = r.Quantity,
                Price = r.BeerPrice
            }));
    }

    internal static Shared.Entities.Availability MapToSharedDto(this Entities.Warehouses.Availability availability)
    {
        return Shared.Entities.Availability.Create(availability.BeerId, availability.BeerName, availability.Quantity);
    }
}