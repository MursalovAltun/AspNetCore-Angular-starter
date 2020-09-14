using System;
using System.Reflection;
using Application.Auth;
using Application.Auth.Identity;
using Application.Auth.Webauthn;
using Application.Components.Captcha;
using Application.Components.PushNotifications;
using Application.Components.TodoItems;
using Application.ExternalApi.TodoItems;
using AutoMapper;
using EF.Models;
using EF.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEfModels(configuration);

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddHttpContextAccessor();

            services.AddIdentity<User, Role>(
                    options =>
                    {
                        options.User.RequireUniqueEmail = true;
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 1;
                        options.Password.RequiredUniqueChars = 0;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                    }
                )
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<UserManager>();

            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });

            var externalApiSection = configuration.GetSection("ExternalApiTodoItems");

            services.AddHttpClient<ITodoItemsService, ExternalApiTodoItemsService>(c =>
            {
                c.BaseAddress = new Uri(externalApiSection.Get<ExternalTodoItemsApiOptions>().Host);
            });

            services.AddHttpClient<ICaptchaValidationService, CaptchaValidationService>();

            services.AddConfigurations(configuration);

            return services;
        }

        private static IServiceCollection AddConfigurations(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtTokenOptions>(configuration.GetSection("JwtToken"));

            services.Configure<CaptchaOptions>(configuration.GetSection("Captcha"));

            services.Configure<SignInConfiguration>(configuration.GetSection("SignIn"));

            services.Configure<PushNotificationsConfiguration>(configuration.GetSection("PushNotifications"));

            services.Configure<CommonConfiguration>(configuration.GetSection("Common"));

            services.Configure<WebauthnConfiguration>(configuration.GetSection("Webauthn"));

            services.Configure<ExternalTodoItemsApiOptions>(configuration.GetSection("ExternalApiTodoItems"));

            return services;
        }
    }
}