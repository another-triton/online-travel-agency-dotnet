using RezLive.Adapter.Api.Common;
using RezLive.Adapter.Api.Repository.Contracts;

namespace RezLive.Adapter.Api.Repository;
public class SimulatorRepository(HttpClient client) : ISimulatorRepository
{
    public async Task<string> Build()
    {
        return await client.GetStringAsync("build");
    }

    public async Task<string> GetAccomodationBySupplierAsync(int supplierId)
    {
        string url = $"{Constants.ACCOMODATION_BY_SUPPLIER}";
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new StringContent($"{supplierId}", null, "application/json");
        var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        return content;
    }

    public async Task<string> Static3()
    {
        return await client.GetStringAsync("static3");
    }
}