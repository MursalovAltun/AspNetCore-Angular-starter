using System;
using Microsoft.EntityFrameworkCore;

namespace Application.UnitTests.Fixtures
{
    public class TestAppDbContextFactory
    {
        public static TestAppDbContext Create()
        {
            var options = new DbContextOptionsBuilder<TestAppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseLazyLoadingProxies()
                .Options;

            var context = new TestAppDbContext(options);

            context.Database.EnsureCreated();

            context.SaveChanges();

            return context;
        }
    }
}