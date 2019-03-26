using HotelsApi.DataAccess;
using Moq;
using Moq.Protected;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HotelsApi.Tests
{
    public class DemoDataReaderTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1001)]
        public void CsvImportNumberOfHotelsWrong(int numberOfHotels)
        {
            var subjectUnderTest = new DemoDataReader(Mock.Of<IHttpClientFactory>());
            Assert.ThrowsAsync<ArgumentException>(() => subjectUnderTest.GetHotelsAsync(numberOfHotels));
        }

        [Fact]
        public async Task TestCsvImport()
        {
            const string csvData =
                "hotel_id,country,city,address,street,hotel_name,postal_code,image,description\n" +
                "1, Poland, Borek Wielkopolski, 75243 Banding Crossing, Mesta, The Borek Wielkopolski City Hotel, 63 - 810, http://dummyimage.com/500x500.png/5fa2dd/ffffff, \"Testdescription\"\n" +
                "2, United States, Scottsdale, 9357 Meadow Ridge Alley, Lukken, United States Superior Grand Hotel, 85260, http://dummyimage.com/500x500.png/cc0000/ffffff, \"Testdescription\"\n";

            // Setup mock object for HTTP response
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(csvData)
               })
               .Verifiable();
            var handlerFactoryMock = new Mock<IHttpClientFactory>();
            handlerFactoryMock.Setup(foo => foo.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(handlerMock.Object) { BaseAddress = new Uri("https://dummy.com") })
                .Verifiable();

            // Execute method
            var subjectUnderTest = new DemoDataReader(handlerFactoryMock.Object);
            var hotels = await subjectUnderTest.GetHotelsAsync(2);

            // Check results
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>());
            handlerFactoryMock.Verify(foo => foo.CreateClient(It.IsAny<string>()), Times.Once());
            Assert.NotNull(hotels);
            Assert.Equal(2, hotels.Count());
        }
    }
}
