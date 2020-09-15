using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi.Configurations
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerDocument();

            return services;
        }

        public static IApplicationBuilder UseConfiguredSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment()) return app;

            app.UseOpenApi();
            app.UseSwaggerUi3();

            return app;
        }
    }
}