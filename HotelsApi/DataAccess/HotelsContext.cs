using HotelsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelsApi.DataAccess
{
    public class HotelsContext : DbContext
    {
        public HotelsContext()
        {

        }

        public HotelsContext(DbContextOptions<HotelsContext> options) : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
    }
}
