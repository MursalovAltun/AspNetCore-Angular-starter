using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Filters;

namespace WebApi.Configurations
{
    public static class ControllersConfiguration
    {
        public static IServiceCollection ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
                {
                    var authorizationPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(authorizationPolicy));
                    options.Filters.Add<BadRequestExceptionFilter>();
                })
                .AddFluentValidation()
                .AddNewtonsoftJson();

            return services;
        }

        public static IApplicationBuilder UseConfiguredControllers(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            return app;
        }
    }
}