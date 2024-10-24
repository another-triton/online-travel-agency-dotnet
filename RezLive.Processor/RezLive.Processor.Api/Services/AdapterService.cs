using Konscious.Security.Cryptography;
using Microsoft.Extensions.Options;
using RezLive.Processor.Api.DTOs;
using RezLive.Processor.Api.Repository.Contracts;
using RezLive.Processor.Api.Services.Contracts;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace RezLive.Processor.Api.Services;

public class AdapterService(IAdapterRepository adapterClient, IOptions<ProcessorSettings> processorSettings) : IAdapterService
{
    private readonly ProcessorSettings _processorSettings = processorSettings.Value;

    private static string xmlTemplate = $@"<HotelFindResponse time=""0.21500015258789"" ipaddress=""14.140.153.130"" count=""0""> 
                <ArrivalDate>01/06/2024</ArrivalDate>
                <DepartureDate>10/06/2024</DepartureDate>
                <Currency>INR</Currency>
                <GuestNationality>IN</GuestNationality>
                <SearchSessionId>17168872488751716887248949665</SearchSessionId>
                <Hotels></Hotels></HotelFindResponse>";
    public async Task<string> GetAccomodations()
    {
        ///We have total 10 numbers of supplier
        int[] supplierList = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        ///Picking up n number of supplier for any Agent Request
        ///I.e. Random Number = 6, It will pick any 6 in series: 8,9,10,1,2,3
        ///MaxNoOfSupplier is configurable from appsettings.json
        Random random = new();
        int maxNoOfSupplier = random.Next(_processorSettings.NoOfSuppliers.Min, _processorSettings.NoOfSuppliers.Max);
        int supplierFromIndex = random.Next(0, supplierList.Length - 1);

        ///Preparing a list of suppliers to execute request for each Agent Request.
        List<Task<string>> tasks = [];
        for (int i = 0; i < maxNoOfSupplier; i++)
        {
            tasks.Add(adapterClient.GetAccomodationBySupplierAsync(supplierList[supplierFromIndex]));
            // Example Scenario: if start index is 6 and we need to pick 8 supplier then we need to start with 1 after 10. 6,7,8,9,10,1,2,3
            supplierFromIndex = (supplierFromIndex + 1) % supplierList.Length;
        }

        ///Executing request for all suppliers
        var results = await Task.WhenAll(tasks);


        ///After getting the response, we need to merge all the hotels together in a single XML Document
        XDocument xDocument = XDocument.Parse(xmlTemplate);
        XElement? xHotels = xDocument?.Root?.Descendants("Hotels").FirstOrDefault();
        //XElement? xReference = null;
        foreach (string result in results)
        {
            XElement xResult = XElement.Parse(result);            
            //if (xReference == null)
            //{
            //    xReference = xResult;
            //}
            xHotels?.Add(xResult.Elements());
        }

        int hotelCount = xDocument.Root.Descendants("Hotels").FirstOrDefault().Elements().Count();

        xDocument.Root.SetAttributeValue("count", hotelCount);

        PerformCPUAndMemoryUsageAsync(results[0]);
        
        return xDocument.ToString();
    }

    /// <summary>
    /// Monitors CPU usage by simulating work for a random duration within the configured range.
    /// </summary>
    /// <param name="xmlDocument">The XML document to use for simulating work.</param>
    private void PerformCPUAndMemoryUsageAsync(string xmlResult)
    {
        if (_processorSettings.CPUUsageInMilliseconds.Min == 0 && _processorSettings.CPUUsageInMilliseconds.Max == 0)
            return;

        var minCpuUsageInMilliseconds = _processorSettings.CPUUsageInMilliseconds.Min;
        var maxCpuUsageInMilliseconds = _processorSettings.CPUUsageInMilliseconds.Max;
        Random random = new();
        var loopTillTime = DateTime.Now.AddMilliseconds(random.Next(minCpuUsageInMilliseconds, maxCpuUsageInMilliseconds));
        //var loopTillTime = DateTime.Now.AddMilliseconds(1000);

        //int counter = 0;
        while (loopTillTime > DateTime.Now)
        {
            //// Simulate CPU work without allocating memory
            //xmlResult.SetAttributeValue("Timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFFFFF"));
            _ = GetHashCode(xmlResult);
            //_ = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFFFFF").GetHashCode();

            //counter++;
        }
            
        //Debug.WriteLine($"Loop Count: {counter}");
    }

    public  int GetHashCode(string xml)
    {
        byte[] data = Encoding.UTF8.GetBytes(xml);
        var argon2 = new Argon2id(data)
        {
            Salt = Encoding.UTF8.GetBytes("somesalt"),
            DegreeOfParallelism = 1, // High degree of parallelism for CPU usage
            MemorySize = 1024, // Large memory size for complexity
            Iterations = 1 // Increase iterations for CPU usage
        };

        byte[] hash = argon2.GetBytes(32);
        return BitConverter.ToInt32(hash, 0);
    }

    public class OptimizedStringBuilder
    {
        private readonly List<string> _chunks = [];
        private int _totalLength = 0;
        public void Append(string str)
        {
            _chunks.Add(str);
            _totalLength += str.Length;
        }
        public string ToStringOptimized()
        {
            var finalArray = new char[_totalLength];
            int position = 0;
            foreach (var chunk in _chunks)
            {
                chunk.CopyTo(0, finalArray, position, chunk.Length);
                position += chunk.Length;
            }
            return new string(finalArray);
        }
    }

    #region Static Services

    public async Task<string> Static2()
    {
        return await adapterClient.Static2();
    }

    public async Task<string> Static3()
    {
        return await adapterClient.Static3();
    }

    public async Task<string> Build()
    {
        return await adapterClient.Build();
    }

    #endregion
}