using CompressedStaticFiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Configurations
{
    public static class StaticFilesConfiguration
    {
        public static IServiceCollection AddConfiguredStaticFiles(this IServiceCollection services)
        {
            services.AddCompressedStaticFiles();

            return services;
        }
        
        public static IApplicationBuilder UseConfiguredStaticFiles(this IApplicationBuilder app)
        {
            app.UseDefaultFiles();

            app.UseCompressedStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = new FileExtensionContentTypeProvider
                {
                    Mappings = {[".webmanifest"] = "text/plain"}
                }
            });

            return app;
        }
    }
}