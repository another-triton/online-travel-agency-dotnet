namespace RezLive.Simulator.Domain.Services.Contracts;
public interface ISupplierService
{
    Task<string> GetAccomodationsAsync(int supplierId);
}