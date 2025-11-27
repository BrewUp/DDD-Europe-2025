namespace BrewUp.Persistence.Sales;

internal static class SaleRepository
{
    private static string FileName(string id) => $"{FileBasedDbConfiguration.DbRoot}/sales-entity-{id}.json";

    internal static async Task InsertSalesOrder(Shared.Entities.SalesOrder salesOrder)
    {
        await File.WriteAllTextAsync(FileName(salesOrder.Id), salesOrder.Serialized());
    }
}
