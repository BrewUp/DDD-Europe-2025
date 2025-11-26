using BrewUp.Persistence.Services;

namespace BrewUp.Persistence.SalesOrder.Services;

public delegate Task SaveEntityToFile(Entities.Sales.SalesOrder entity);

public static class SaleRepositoryStatic
{
    internal static string FileName(string id) => $"{IRepository.DbRoot}/sales-entity-{id}.json";

    internal static readonly SaveEntityToFile SaveEntityToFile = async entity =>
    {
        await File.WriteAllTextAsync(FileName(entity.Id), entity.Serialized());
    };
}