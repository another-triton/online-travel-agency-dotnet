using RezLive.Processor.Api.Common;
using RezLive.Processor.Api.Repository.Contracts;

namespace RezLive.Processor.Api.Repository;
public class AdapterRepository(HttpClient client) : IAdapterRepository
{
    
    public async Task<string> GetAccomodationBySupplierAsync(int supplierId)
    {
        string url = $"{Constants.ACCOMODATION_BY_SUPPLIER}/{supplierId}";


        return await client.GetStringAsync(url);
    }

    public async Task<string> Static2()
    {
        var response = await client.GetStringAsync("static2") ?? string.Empty;
        
        return response;
    }

    public async Task<string> Static3()
    {
        var response = await client.GetStringAsync("static3") ?? string.Empty;

        return response;
    }

    public async Task<string> Build()
    {
        var response = await client.GetStringAsync("build") ?? string.Empty;
        return response;
    }
}