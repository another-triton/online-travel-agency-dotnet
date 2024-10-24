using Amazon;

namespace RezLive.Processor.Api.DTOs;

public class AdapterSettings
{
    public string BaseURL { get; set; } = string.Empty;

};
public record SupplierAdapterResponse(int SupplierId, string Xml);

public class SerilogAWSCloudWatchSettings
{
    public string profile { get; set; } = string.Empty;
    public string logGroup { get; set; } = string.Empty;
    public string logStreamPrefix { get; set; } = string.Empty;
    public RegionEndpoint region { get; set; } = RegionEndpoint.EUWest1;
}

public class CPUUsage
{
    public int Min { get; set; }
    public int Max { get; set; }
}
public class ProcessorSettings
{
    public CPUUsage CPUUsageInMilliseconds { get; set; } = null!;
    public NoOfSupplier NoOfSuppliers { get; set; } = null!;
}

public class NoOfSupplier
{
    public int Min { get; set; } = 3;
    public int Max { get; set; } = 6;
}