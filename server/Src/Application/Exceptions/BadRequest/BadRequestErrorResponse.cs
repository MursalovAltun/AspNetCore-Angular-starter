using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Application.Exceptions.BadRequest
{
    public class BadRequestErrorResponse
    {
        public BadRequestErrorResponse()
        {
        }

        public BadRequestErrorResponse(List<BadRequestPropertyError> propertyErrors)
        {
            PropertyErrors = propertyErrors;
        }

        public BadRequestErrorResponse(IList<ValidationFailure> errors)
        {
            PropertyErrors = errors
                .Where(e => !string.IsNullOrEmpty(e.PropertyName))
                .Select(e => new BadRequestPropertyError(e));
        }

        public object Data { get; set; }
        public string Code { get; set; }
        public IEnumerable<BadRequestPropertyError> PropertyErrors { get; set; } = new List<BadRequestPropertyError>();
    }
}