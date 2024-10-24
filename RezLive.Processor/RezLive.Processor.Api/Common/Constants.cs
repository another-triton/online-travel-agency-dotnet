namespace RezLive.Processor.Api.Common;

public static class Constants
{
    //Route
    public const string ACCOMODATION_BY_SUPPLIER = "accomodation-by-supplier";   

    //appsettings.json
    public const string ADAPTER_SETTINGS_KEY = "AdapterSettings";
    public const string PROCESSOR_SETTINGS_KEY = "ProcessorSettings";

    //serilog configurations
    public const string CORRELATION_ID_HEADER = "rezlive-correlation-id";
    public const string SERILOG_AWSCLOUDWATCH_SETTINGS = "SerilogAWSCloudWatchSettings";    
    public const string SERILOG_DATE_FORMAT_SHORT = "yyyy-MM-dd";

    public static int RequestCounter = 0;
}