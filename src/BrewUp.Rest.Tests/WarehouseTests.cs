using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Text.Json;
using BrewUp.Shared.Contracts;
using BrewUp.Shared.CustomTypes;
using Xunit;

namespace BrewUp.Rest.Tests;

[ExcludeFromCodeCoverage]
[Collection( "Integration Fixture" )]
public class WarehouseTests (AppHttpClientFixture integrationFixture)
{
    [Fact]
    public async Task Can_HandleSetAvailabilities()
    {
        SetAvailabilityJson body = new ("fd23d06e-e5eb-4ecc-93a6-516caf4a08b8", "beername", new Quantity(12, "bottles"));
        
        var stringJson = JsonSerializer.Serialize(body);
        var httpContent = new StringContent(stringJson, Encoding.UTF8, "application/json");
        var postResult = await integrationFixture.Client.PostAsync("/v1/warehouses/availabilities", httpContent);
        
        Assert.Equal(HttpStatusCode.OK, postResult.StatusCode);
    }
}
