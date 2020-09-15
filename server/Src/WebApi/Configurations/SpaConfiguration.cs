using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace WebApi.Configurations
{
    public static class SpaConfiguration
    {
        public static IApplicationBuilder UseSpaRedirection(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 404)
                {
                    context.HttpContext.Response.StatusCode = 200;
                    await context.HttpContext.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
                }
            });

            return app;
        }
    }
}