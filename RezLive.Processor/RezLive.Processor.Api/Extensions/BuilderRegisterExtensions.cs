using Amazon.CloudWatchLogs;
using RezLive.Processor.Api.DTOs;
using Serilog;
using RezLive.Processor.Api.Common;
using Serilog.Sinks.AwsCloudWatch;

namespace RezLive.Processor.Api.Extensions;

public static class BuilderRegisterExtensions
{
    public static WebApplicationBuilder RegisterSeriLog(this WebApplicationBuilder builder) 
    {
        // Get awsCredentials using the AWS profile configured on machine and Create an AWS CloudWatch client
        var serilogAWSCloudWatchSettings = builder.Configuration.GetSection(Constants.SERILOG_AWSCLOUDWATCH_SETTINGS).Get<SerilogAWSCloudWatchSettings>();

        ///Uncomment following section to use the credentials stored in AWS
        /*
        CredentialProfileStoreChain chain = new ();
        chain.TryGetAWSCredentials(serilogAWSCloudWatchSettings?.profile, out AWSCredentials awsCredentials);
        var client = new AmazonCloudWatchLogsClient(credentials: awsCredentials, serilogAWSCloudWatchSettings?.region);
        */
        AmazonCloudWatchLogsClient client = new ("AKIAQQPWFRTP5AAFC2P4", "/c3KlvQf/zD7MCYNNo+n+1/mShH4dvK/NL/2cj7q", serilogAWSCloudWatchSettings?.region);

        //Init Serilog
        builder.Host.UseSerilog((context, lc) =>
            lc.ReadFrom.Configuration(context.Configuration)
            .WriteTo.AmazonCloudWatch(
                logGroup: serilogAWSCloudWatchSettings?.logGroup,
                logStreamPrefix: serilogAWSCloudWatchSettings?.logStreamPrefix + DateTime.UtcNow.ToString(Constants.SERILOG_DATE_FORMAT_SHORT),
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose,
                cloudWatchClient: client)
            .Enrich.WithCorrelationIdHeader(Constants.CORRELATION_ID_HEADER)
        );

        return builder;
    }


    public static WebApplicationBuilder ConfigureAppSettings(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AdapterSettings>(builder.Configuration.GetSection(Constants.ADAPTER_SETTINGS_KEY));
        builder.Services.Configure<ProcessorSettings>(builder.Configuration.GetSection(Constants.PROCESSOR_SETTINGS_KEY));
        return builder;
    }
}
