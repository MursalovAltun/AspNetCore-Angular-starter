using AutoMapper;
using Xunit;

namespace Application.UnitTests.AutoMapper
{
    public class AutoMapperProfilesTests
    {
        [Fact]
        public void Should_BeValid()
        {
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(AutoRegistrationModule).Assembly);
            });

            configurationProvider.AssertConfigurationIsValid();
        }
    }
}