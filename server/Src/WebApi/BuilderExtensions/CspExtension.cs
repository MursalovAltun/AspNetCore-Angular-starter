using Microsoft.AspNetCore.Builder;

namespace WebApi.BuilderExtensions
{
    public static class CspExtension
    {
        public static IApplicationBuilder UsePreconfiguredCsp(this IApplicationBuilder app)
        {
            const string googleRecaptcha = "https://www.google.com/recaptcha/";
            const string gstaticRecaptcha = "https://www.gstatic.com/recaptcha/";
            const string googleapis = "https://fonts.googleapis.com/";
            const string fontsGstatic = "https://fonts.gstatic.com/";

            return app.UseWhen(
                context => context.Request.Path != "/ngsw-worker.js",
                appBuilder => appBuilder.UseCsp(options => options
                    .BaseUris(x => x.Self())
                    .DefaultSources(s => s.Self())
                    .ScriptSources(s => s.Self().CustomSources(gstaticRecaptcha, googleRecaptcha))
                    .FrameSources(s => s.Self().CustomSources(googleRecaptcha))
                    .StyleSources(x => x.Self()
                            .CustomSources(googleapis)
                            .UnsafeInline() // https://github.com/angular/angular/issues/6361, https://github.com/angular/angular/issues/26152
                    )
                    .ConnectSources(x => x.Self())
                    .FontSources(s => s.Self().CustomSources(googleapis, fontsGstatic))
                    .ImageSources(s => s.Self().CustomSources("blob:", "data:"))
                    .FormActions(s => s.None())
                    .FrameAncestors(s => s.None())
                    .ObjectSources(s => s.None())
                    .WorkerSources(x => x.Self())
                    .BlockAllMixedContent()
                )
            );
        }
    }
}