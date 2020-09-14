using System.Linq;
using Common;
using FluentValidation.Results;

namespace Application.Exceptions.BadRequest
{
    public class BadRequestPropertyError
    {
        public string Code { get; set; }
        public string PropertyName { get; set; }

        public BadRequestPropertyError()
        {
        }

        public BadRequestPropertyError(ValidationFailure validationFailure, string propertyName = null)
        {
            Code = validationFailure.ErrorCode;
            PropertyName = propertyName ??
                           validationFailure.PropertyName.First().ToString().ToLower() +
                           validationFailure.PropertyName.Substring(1);
        }

        public BadRequestPropertyError(string propertyName, ErrorCodes errorCode)
        {
            Code = errorCode.ToStringName();
            PropertyName = propertyName;
        }
    }
}