using BrewUp.Infrastructure.TextBasedDb;
using BrewUp.Persistence.SalesOrder.Services;

namespace BrewUp.Rest;

public static class CompositionRootBuilder
{
    public record CompositionRoot(CreateSalesOrder CreateSalesOrder);

    public static CompositionRoot Build()
    {
        return new CompositionRoot(
            CreateSalesOrder: SalesOrderServiceStatic.CreateSalesOrder(
                new SaleRepository(),
                new WarehouseRepository()
            ));
    }
}