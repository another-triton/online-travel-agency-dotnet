using Microsoft.Extensions.Options;
using RezLive.Simulator.Domain.DTOs;
using RezLive.Simulator.Domain.Services.Contracts;

namespace RezLive.Simulator.Domain.Services;
public class SupplierService(IOptions<AppSettings> options) : ISupplierService
{
    private readonly AppSettings appSettings = options.Value ?? AppSettings.GetDefault();
    public async Task<string> GetAccomodationsAsync(int supplierId)
    {
        ///Delay in Response
        if (appSettings.RandomDelayInApiResponse.Max > appSettings.RandomDelayInApiResponse.Min)
        {
            Random random = new();
            int delay = random.Next(appSettings.RandomDelayInApiResponse.Min, appSettings.RandomDelayInApiResponse.Max);
            if (delay > 0)
            {
                await Task.Delay(delay);
            }
        }
        var supplierIndex = Array.IndexOf(SupplierData.suppliers, supplierId);

        if (supplierIndex >= 0)
        {
            return Convert.ToString(SupplierData.xmlList[supplierIndex]);
        }
        return "";
    }
}