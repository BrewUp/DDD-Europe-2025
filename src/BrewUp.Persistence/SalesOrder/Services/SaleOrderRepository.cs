using BrewUp.Persistence.Services;

namespace BrewUp.Persistence.SalesOrder.Services;

public delegate Task SaveSalesOrder(Entities.Sales.SalesOrder entity);

public static class SaleOrderRepository
{
    private static string FileName(string id) => $"{IRepository.DbRoot}/sales-entity-{id}.json";

    public static readonly SaveSalesOrder SaveSalesOrder = async entity =>
    {
        await File.WriteAllTextAsync(FileName(entity.Id), entity.Serialized());
    };
}