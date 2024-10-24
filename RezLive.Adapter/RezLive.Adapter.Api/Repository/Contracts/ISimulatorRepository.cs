namespace RezLive.Adapter.Api.Repository.Contracts;

public interface ISimulatorRepository
{
    Task<string> GetAccomodationBySupplierAsync(int supplierId);
    Task<string> Static3();
    Task<string> Build();
}