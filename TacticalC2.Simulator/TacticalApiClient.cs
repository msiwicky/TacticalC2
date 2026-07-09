using System.Net.Http.Json;

namespace TacticalC2.Simulator;

public class TacticalApiClient(IHttpClientFactory httpClientFactory)
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("TacticalApi");

    public async Task<Guid> RegisterUnit(SimulatedUnit unit)
    {
        var response = await _client.PostAsJsonAsync("/api/units", new
        {
            Name = unit.Name,
            Type = 0,
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
}