using System;
using System.Linq;
using Autofac;
using AutoMapper;
using EF.Models;

namespace Application.UnitTests.Fixtures
{
    public class TestFixture
    {
        public AppDbContext Context { get; }
        public IMapper Mapper { get; }

        public TestFixture(params Type[] profileTypes)
        {
            if (profileTypes.Any(t => t.BaseType != typeof(Profile)))
            {
                throw new ArgumentException("Argument parameter must inherit AutoMapper.Profile");
            }

            Context = TestAppDbContextFactory.Create();

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                var profiles = profileTypes
                    .Select(type => Activator.CreateInstance(type) as Profile)
                    .ToList();

                cfg.AddProfiles(profiles);
            });

            Mapper = configurationProvider.CreateMapper();
        }

        public Action<ContainerBuilder> BeforeBuild => cfg =>
        {
            cfg.RegisterInstance(Context).As<AppDbContext>();
            cfg.RegisterInstance(Mapper).As<IMapper>();
        };
    }
}