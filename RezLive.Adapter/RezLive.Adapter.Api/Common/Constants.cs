namespace RezLive.Adapter.Api.Common;
public static class Constants
{
    //Route
    public const string ACCOMODATION_BY_SUPPLIER = "accomodation-by-supplier";

    //appsettings.json
    public const string SUPPLIER_SETTINGS_KEY = "SimulatorSettings";

    //serilog configurations
    public const string CORRELATION_ID_HEADER = "rezlive-correlation-id";
    public const string SERILOG_AWSCLOUDWATCH_SETTINGS = "SerilogAWSCloudWatchSettings";
    public const string SERILOG_DATE_FORMAT_SHORT = "yyyy-MM-dd";
}