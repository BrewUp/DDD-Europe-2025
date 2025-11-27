namespace BrewUp.Persistence.Warehouses;

internal static class WarehouseRepositoryStatic
{
    private static string FileName(string id) => $"{IRepository.DbRoot}/warehouse-entity-{id}.json";

    internal static async Task<Shared.Entities.Availability> GetAvailabilityById(string id)
    {
        if (!File.Exists(FileName(id))) return await Task.FromResult<Shared.Entities.Availability>(null);

        return (await File.ReadAllTextAsync(FileName(id))).Deserialized<Shared.Entities.Availability>();
    }

    internal static readonly InsertAvailability InsertAvailability = async availability =>
    {
        await File.WriteAllTextAsync(FileName(availability.Id), availability.Serialized());
    };
}
