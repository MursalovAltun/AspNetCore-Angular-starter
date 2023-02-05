using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace WebApi.Configurations
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(document =>
            {
                document.AddSecurityDefinition("JWT", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });
            });

            return services;
        }

        public static IApplicationBuilder UseConfiguredSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment()) return app;

            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}