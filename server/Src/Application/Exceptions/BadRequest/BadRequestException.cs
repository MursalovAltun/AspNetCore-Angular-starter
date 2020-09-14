using System;
using Common;

namespace Application.Exceptions.BadRequest
{
    public class BadRequestException : Exception
    {
        private readonly object _data;
        private readonly string _code;

        public BadRequestException(ErrorCodes errorCode, object data = null)
        {
            _code = errorCode.ToStringName();
            _data = data;
        }

        public BadRequestErrorResponse Response => new BadRequestErrorResponse
        {
            Code = _code,
            Data = _data,
        };
    }
}