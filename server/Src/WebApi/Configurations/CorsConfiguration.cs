using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi.Configurations
{
    public static class CorsConfiguration
    {
        public static IServiceCollection ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p => p
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
                options.AddPolicy("Default", p => { });
            });

            return services;
        }

        public static IApplicationBuilder UseConfiguredCors(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(env.IsDevelopment() ? "AllowAll" : "Default");

            return app;
        }
    }
}