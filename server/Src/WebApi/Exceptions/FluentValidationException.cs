using System;
using System.Collections.Generic;
using FluentValidation.Results;

namespace WebApi.Exceptions
{
    public class FluentValidationException : Exception
    {
        public IList<ValidationFailure> Errors { get; set; }

        public FluentValidationException(IList<ValidationFailure> errors)
        {
            Errors = errors;
        }
    }
}