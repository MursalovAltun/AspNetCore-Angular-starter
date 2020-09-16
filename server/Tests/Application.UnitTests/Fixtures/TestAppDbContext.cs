using EF.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.UnitTests.Fixtures
{
    public class TestAppDbContext : AppDbContext
    {
        public override void Dispose()
        {
            Database.EnsureDeleted();
            base.Dispose();
        }

        public TestAppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}