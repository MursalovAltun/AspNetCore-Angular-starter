using CompressedStaticFiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;

namespace WebApi.Configurations
{
    public static class StaticFilesConfiguration
    {
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