using HotelsApi.DataAccess;
using HotelsApi.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace HotelsApi.Tests
{
    public class DemoDataFillerTests
    {
        [Fact]
        public async Task TestFillDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<HotelsContext>();
            optionsBuilder.UseInMemoryDatabase("Hotels");
            var context = new HotelsContext(optionsBuilder.Options);

            var reader = new Mock<DemoDataReader>(MockBehavior.Strict, null);
            reader.Setup(foo => foo.GetHotelsAsync(It.IsAny<int>())).ReturnsAsync(new [] { new Hotel() }).Verifiable();

            var filler = new DemoDataFiller(context, reader.Object);
            await filler.FillDatabaseAsync();

            reader.Verify(foo => foo.GetHotelsAsync(It.IsAny<int>()), Times.Once());
            Assert.Equal(1, await context.Hotels.CountAsync());
        }
    }
}
