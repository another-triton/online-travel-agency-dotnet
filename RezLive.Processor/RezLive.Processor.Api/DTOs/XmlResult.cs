using System.Xml.Serialization;

namespace RezLive.Processor.Api.DTOs;

public class XmlResult<T>(T result) : IResult
{
    private static readonly XmlSerializer Serializer = new(typeof(T));

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        using var ms = new MemoryStream();

        // Serialize the object synchronously then rewind the stream
        Serializer.Serialize(ms, result);
        ms.Position = 0;

        httpContext.Response.ContentType = "application/xml";
        await ms.CopyToAsync(httpContext.Response.Body);
    }
}