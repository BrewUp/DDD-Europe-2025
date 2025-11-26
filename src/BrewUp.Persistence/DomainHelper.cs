using BrewUp.Persistence.Entities.Sales;
using BrewUp.Shared.Contracts;

namespace BrewUp.Persistence;

public static class DomainHelper
{
    internal static SalesOrderRow MapToDomainRow(this SalesOrderRowJson json)
    {
        return SalesOrderRow.CreateSalesOrderRow(json.BeerId, json.BeerName, json.Quantity, json.Price);
    }

    internal static IEnumerable<SalesOrderRow> MapToDomainRows(this IEnumerable<SalesOrderRowJson> json)
    {
        return json.Select(r => SalesOrderRow.CreateSalesOrderRow(r.BeerId, r.BeerName, r.Quantity, r.Price));
    }

    internal static Shared.Entities.SalesOrder MapToSharedDto(this Entities.Sales.SalesOrder salesOrder)
    {
        return Shared.Entities.SalesOrder.Create(salesOrder.SalesOrderId, salesOrder.SalesOrderNumber,
            salesOrder.OrderDate, salesOrder.CustomerId, salesOrder.CustomerName,
            salesOrder.Rows.Select(r => new SalesOrderRowJson
            {
                BeerId = r.BeerId,
                BeerName = r.BeerName,
                Quantity = r.Quantity,
                Price = r.BeerPrice
            }));
    }

    internal static Shared.Entities.Availability MapToSharedDto(this Entities.Warehouses.Availability availability)
    {
        return Shared.Entities.Availability.Create(availability.BeerId, availability.BeerName, availability.Quantity);
    }
}
