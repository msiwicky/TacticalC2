using System.Net.Http.Json;

namespace TacticalC2.Simulator;

public class TacticalApiClient(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("TacticalApi");
    
    public async Task<List<ApiUnitDto>> GetExistingUnitsAsync()
    {
        var response = await _client.GetFromJsonAsync<List<ApiUnitDto>>("/api/units");
        return response ?? [];
    }

    public async Task<Guid> RegisterUnit(SimulatedUnit unit)
    {
        var response = await _client.PostAsJsonAsync("/api/units", new
        {
            Name = unit.Name,
            Type = MapUnitType(unit.Type),
            Latitude = unit.Latitude,
            Longitude = unit.Longitude,
            Heading = unit.Heading,
            Speed = unit.Speed
        });

       return await response.Content.ReadFromJsonAsync<Guid>();
    }

    public Task SendPositionUpdateAsync(SimulatedUnit unit)
    {
        return _client.PutAsJsonAsync($"/api/units/{unit.Id}/position", new
        {
            Latitude = unit.Latitude,
            Longitude = unit.Longitude,
            Heading = unit.Heading,
            Speed = unit.Speed
        });
    }
    
    private static int MapUnitType(string type) => type switch
    {
        "Drone" => 0,
        "Vehicle" => 1,
        "Infantry" => 2,
        _ => throw new ArgumentException($"Unknown unit type: {type}")
    };
}