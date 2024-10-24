namespace RezLive.Simulator.Domain.DTOs
{
    public class AppSettings
    {
        public RandomDelayInApiResponseSetting RandomDelayInApiResponse { get; set; } = null!;
        public string XmlPath { get; set; } = string.Empty!;

        public static AppSettings GetDefault()
        {
            return new AppSettings
            {
                XmlPath = "/SupplierXml",
                RandomDelayInApiResponse = new()
                {
                    Min = 1,
                    Max = 10
                }
            };
        }
    }

    public class RandomDelayInApiResponseSetting
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
