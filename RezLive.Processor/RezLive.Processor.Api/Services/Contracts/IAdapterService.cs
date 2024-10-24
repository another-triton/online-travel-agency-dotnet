using System.Text;
using System.Xml;

namespace RezLive.Processor.Api.Services.Contracts;
public interface IAdapterService
{
    Task<string> GetAccomodations();
    Task<string> Static2();
    Task<string> Static3();
    Task<string> Build();
}