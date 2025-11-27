using System.Linq.Expressions;
using BrewUp.Persistence.Sales.Queries;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.Entities;
using Serilog.Core;

namespace BrewUp.Persistence.Sales.Services;

public delegate Task<PagedResult<SalesOrderJson>> GetSalesOrders(int page, int pageSize);

public static class SalesQueryService
{

    public static GetSalesOrders GetSalesOrders(Logger logger, GetSalesOrderByFilter getSalesOrderByFilter) => async (page, pageSize) =>
    {
        try
        {
            var salesOrders = await getSalesOrderByFilter(null, page, pageSize);

            return salesOrders.TotalRecords > 0
                ? new PagedResult<SalesOrderJson>(salesOrders.Results.Select(r => r.ToJson()), salesOrders.Page, salesOrders.PageSize, salesOrders.TotalRecords)
                : new PagedResult<SalesOrderJson>(Enumerable.Empty<SalesOrderJson>(), 0, 0, 0);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error reading SalesOrders");
            throw;
        }
    };
}
