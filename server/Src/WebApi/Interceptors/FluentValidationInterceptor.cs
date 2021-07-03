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
        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }

        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext,
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