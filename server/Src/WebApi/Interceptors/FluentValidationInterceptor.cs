using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;

namespace WebApi.Interceptors
{
    [As(typeof(IValidatorInterceptor))]
    public class FluentValidationInterceptor : IValidatorInterceptor
    {
        public IValidationContext BeforeMvcValidation(
            ControllerContext controllerContext,
            IValidationContext validationContext)
        {
            return validationContext;
        }

        public ValidationResult AfterMvcValidation(ControllerContext controllerContext,
            IValidationContext validationContext,
            ValidationResult result)
        {
            if (result.Errors.Count > 0)
            {
                throw new FluentValidationException(result.Errors);
            }

            return result;
        }
    }
}