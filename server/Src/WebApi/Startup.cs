using Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi.Configurations;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureControllers();

            services.AddApplication(Configuration);

            services.ConfigureAuth(Configuration);

            services.ConfigureSwagger();

            services.ConfigureCors();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseConfiguredSwagger(env);

            app.UseConfiguredCors(env);

            app.UsePreconfiguredCsp();

            app.UseConfiguredStaticFiles();

            app.UseSpaRedirection(env);

            app.UseConfiguredAuth();

            app.UseConfiguredControllers();
        }
    }
}