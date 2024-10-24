namespace RezLive.Adapter.Api.Services.Contracts;
public interface ISimulatorService
{
    Task<string> GetAccomodationBySupplier(int supplierId);
    Task<string> Static3();
    Task<string> Build();
}