using Common;
using FluentValidation;

namespace Application
{
    public static class ErrorCodesValidationExtension
    {
        public static IRuleBuilderOptions<T, TProperty> WithErrorCode<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule, ErrorCodes errorCode)
        {
            return rule.WithErrorCode(errorCode.ToStringName());
        }
    }
}