using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.OptionsValidation
{
    public static class OptionsValidationServiceCollectionExtensions
    {
        public static void ConfigureAndValidate<T>(this IServiceCollection services,
            IConfigurationSection config)
            where T : class, new()
        {
            if (!config.Exists()) return;

            services.Configure<T>(config);
            config.Get<T>().Validate();
        }

        private static void Validate(this object @this)
        {
            var validation = new List<ValidationResult>();

            if (!Validator.TryValidateObject(@this, new ValidationContext(@this), validation, true))
            {
                throw new ValidationException($"{@this.GetType()}");
            }
        }
    }
}