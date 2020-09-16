using System;
using System.Reflection;
using Application.Auth;
using Application.Auth.Identity;
using Application.Auth.Webauthn;
using Application.Components.Captcha;
using Application.Components.EmailSender;
using Application.Components.PushNotifications;
using Application.Components.TodoItems;
using Application.ExternalApi.TodoItems;
using Application.OptionsValidation;
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
            services.ConfigureAndValidate<JwtTokenOptions>(configuration.GetSection("JwtToken"));

            services.ConfigureAndValidate<CaptchaOptions>(configuration.GetSection("Captcha"));

            services.ConfigureAndValidate<SignInConfiguration>(configuration.GetSection("SignIn"));

            var pushNotificationsConfiguration = configuration.GetSection("PushNotifications");
            services.ConfigureAndValidate<PushNotificationsConfiguration>(pushNotificationsConfiguration);
            
            if (pushNotificationsConfiguration.Get<PushNotificationsConfiguration>()?.UseMailHog ?? false)
            {
                services.AddScoped<IPushNotificationsClient, MailHogPushNotificationsClient>();
            }
            else
            {
                services.AddScoped<IPushNotificationsClient, PushNotificationsClient>();
            }
            
            var emailConfiguration = configuration.GetSection("EmailConfiguration");
            services.ConfigureAndValidate<EmailConfiguration>(emailConfiguration);

            if (emailConfiguration.Get<EmailConfiguration>()?.UseMailHog ?? false)
            {
                services.AddScoped<IEmailService, MailHogEmailService>();
            }
            else
            {
                services.AddScoped<IEmailService, SendGridEmailService>();
                services.AddScoped<ISendGridClientContainer, SendGridClientContainer>();
            }

            services.ConfigureAndValidate<CommonConfiguration>(configuration.GetSection("Common"));

            services.ConfigureAndValidate<WebauthnConfiguration>(configuration.GetSection("Webauthn"));

            services.ConfigureAndValidate<ExternalTodoItemsApiOptions>(configuration.GetSection("ExternalApiTodoItems"));

            return services;
        }
    }
}