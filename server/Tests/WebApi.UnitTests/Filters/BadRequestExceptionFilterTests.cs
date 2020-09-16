using System;
using System.Collections.Generic;
using Application;
using Application.Exceptions.BadRequest;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using UnitTests.Common.Asserts;
using WebApi.Exceptions;
using WebApi.Filters;
using Xunit;

namespace WebApi.UnitTests.Filters
{
    public class BadRequestExceptionFilterUnitTest
    {
        [Fact]
        public void Should_NotChangeContext_On_NonSpecificException()
        {
            var context = GetExceptionContext();

            var exception = new Exception();

            context.Exception = exception;

            var sut = new BadRequestExceptionFilter();

            sut.OnException(context);

            Assert.Equal(context.Exception, exception);
            Assert.Null(context.Result);
        }

        [Fact]
        public void Should_ResetExceptionAndSetResult_On_FluentValidationException()
        {
            var context = GetExceptionContext();

            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("propertyName", "errorMessage")
            };

            var exception = new FluentValidationException(failures);

            context.Exception = exception;

            var sut = new BadRequestExceptionFilter();

            sut.OnException(context);

            Assert.Null(context.Exception);
            Assert.IsType<BadRequestObjectResult>(context.Result);
            Assert.Equal((context.Result as BadRequestObjectResult)?.Value, failures);
        }

        [Fact]
        public void Should_ResetExceptionAndSetResult_On_BadRequestException()
        {
            var context = GetExceptionContext();

            var exception = new BadRequestException(ErrorCodes.LOGIN_FAILED);

            context.Exception = exception;

            var sut = new BadRequestExceptionFilter();

            sut.OnException(context);

            Assert.Null(context.Exception);
            Assert.IsType<BadRequestObjectResult>(context.Result);
            ContentAssert.AreEqual((context.Result as BadRequestObjectResult)?.Value, exception.Response);
        }

        private static ExceptionContext GetExceptionContext()
        {
            return new ExceptionContext(
                new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor()),
                Array.Empty<IFilterMetadata>()
            );
        }
    }
}