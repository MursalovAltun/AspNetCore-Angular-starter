using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EF.Models
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEfModels(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options => options
                .UseSqlServer(connectionString, b => b.MigrationsAssembly("EF.Manager"))
                .UseLazyLoadingProxies()
            );

            return services;
        }
    }
}