﻿using Amazon;

namespace RezLive.Adapter.Api.DTOs
{
    public class SerilogAWSCloudWatchSettings
    {
        public string profile { get; set; } = string.Empty;
        public string logGroup { get; set; } = string.Empty;
        public string logStreamPrefix { get; set; } = string.Empty;
        public RegionEndpoint region { get; set; } = RegionEndpoint.EUWest1;
    }
}