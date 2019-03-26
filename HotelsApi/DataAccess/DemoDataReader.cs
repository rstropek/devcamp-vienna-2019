using CsvHelper;
using CsvHelper.Configuration;
using HotelsApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HotelsApi.DataAccess
{
    public class DemoDataReader
    {
        private const string DemoDataUrl = "api/fcbb26b0?count=COUNT&key=ec7e0e80";
        private readonly IHttpClientFactory HttpClientFactory;

        public DemoDataReader(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        public virtual async Task<IEnumerable<Hotel>> GetHotelsAsync(int numberOfHotels)
        {
            // Note that this function will be much easier to implement with .NET Core 3 and C# 8.
            // You will be able to use async enumerables there.

            if (numberOfHotels < 1 || numberOfHotels > 1000)
            {
                throw new ArgumentException($"{nameof(numberOfHotels)} must be between 1 and 1000");
            }

            var url = DemoDataUrl.Replace("COUNT", numberOfHotels.ToString(CultureInfo.InvariantCulture));
            var client = HttpClientFactory.CreateClient("mockaroo");
            using (var demoDataStream = await client.GetStreamAsync(url))
            using (var demoDataReader = new StreamReader(demoDataStream))
            using (var csv = new CsvReader(demoDataReader, new Configuration { Delimiter = ",", Quote = '\"', TrimOptions = TrimOptions.Trim }))
            {
                await csv.ReadAsync();
                csv.ReadHeader();

                var result = new List<Hotel>();
                while (await csv.ReadAsync())
                {
                    result.Add(new Hotel
                    {
                        ID = csv.GetField<int>("hotel_id"),
                        HotelName = csv.GetField<string>("hotel_name"),
                        Country = csv.GetField<string>("country"),
                        City = csv.GetField<string>("city"),
                        Address = csv.GetField<string>("address"),
                        PostalCode = csv.GetField<string>("postal_code"),
                        Description = csv.GetField<string>("description"),
                        ImageUrl = csv.GetField<string>("image")
                    });
                }

                return result;
            }
        }
    }
}
