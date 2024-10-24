namespace RezLive.Processor.Api.Repository.Contracts;

public interface IAdapterRepository
{
    Task<string> GetAccomodationBySupplierAsync(int supplierId);
    Task<string> Static2();
    Task<string> Static3();
    Task<string> Build();
}