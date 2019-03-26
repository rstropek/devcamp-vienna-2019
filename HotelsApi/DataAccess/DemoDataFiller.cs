using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HotelsApi.DataAccess
{
    public class DemoDataFiller
    {
        private HotelsContext Context;
        private DemoDataReader DemoDataReader;

        public DemoDataFiller(HotelsContext context, DemoDataReader demoDataReader)
        {
            Context = context;
            DemoDataReader = demoDataReader;
        }

        public async Task ClearDatabaseAsync()
        {
            var connection = Context.Database.GetDbConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Hotels";
                await command.ExecuteNonQueryAsync();
            }
        }

        public virtual async Task FillDatabaseAsync()
        {
            var hotels = await DemoDataReader.GetHotelsAsync(1000);
            foreach(var hotel in hotels)
            {
                Context.Hotels.Add(hotel);
            }

            await Context.SaveChangesAsync();
        }
    }
}
