using RezLive.Adapter.Api.Repository.Contracts;
using RezLive.Adapter.Api.Services.Contracts;
using System.Runtime;
using static System.Console;

namespace RezLive.Adapter.Api.Services;
public class SimulatorService(ISimulatorRepository supplierClient) : ISimulatorService
{
    public async Task<string> Build()
    {
        return await supplierClient.Build();
    }

    public async Task<string> GetAccomodationBySupplier(int supplierId)
    {
        try
        {
            return await supplierClient.GetAccomodationBySupplierAsync(supplierId);
        }
        finally
        {
            /*WriteLine($"IsLargeObjectHeapCompactionMode - {GCSettings.LargeObjectHeapCompactionMode}");
            WriteLine($"GCMemoryInfo - {GC.GetGCMemoryInfo(GCKind.Background)}");*/
        }
       
    }

    public async Task<string> Static3()
    {
        return await supplierClient.Static3();
    }
}