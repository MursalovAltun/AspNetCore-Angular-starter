using System;
using System.Threading.Tasks;
using Application;
using Application.Auth;
using Application.Auth.Identity;
using Application.Components.Captcha;
using Application.Exceptions.BadRequest;
using Autofac.Extras.Moq;
using EF.Models.Models;
using Moq;
using UnitTests.Common.Asserts;
using WebApi.Controllers;
using Xunit;

namespace WebApi.UnitTests.Controllers
{
    public class AuthControllerTests
    {
        [Fact]
        public void Should_ThrowException_When_UserDoesNotExist()
        {
            using var mock = AutoMock.GetLoose();

            const string email = "email";

            var request = new AuthenticateRequest
            {
                Password = "password",
                Email = "email",
            };

            mock.Mock<IUserManager>()
                .Setup(manager => manager.FindByEmailAsync(email))
                .ReturnsAsync(null as User);

            var sut = mock.Create<AuthController>();

            ExceptionAssert.ThrowsAsync(new BadRequestException(ErrorCodes.LOGIN_FAILED),
                () => sut.Authenticate(request));
        }

        [Fact]
        public void Should_ThrowException_When_CaptchaRequiredAndTokenIsInvalid()
        {
            using var mock = AutoMock.GetLoose();

            const string email = "email";
            const string token = "token";
            var captchaRequiredUntil = new DateTime();

            var request = new AuthenticateRequest
            {
                Password = "password",
                Email = "email",
                CaptchaToken = token
            };

            var user = new User();

            mock.Mock<IUserManager>()
                .Setup(manager => manager.FindByEmailAsync(email))
                .ReturnsAsync(user);

            mock.Mock<IUserAccessFailedService>()
                .Setup(service => service.IsCaptchaRequired(user))
                .ReturnsAsync(captchaRequiredUntil);

            mock.Mock<ICaptchaValidationService>()
                .Setup(service => service.IsValidAsync(token))
                .ReturnsAsync(false);

            var sut = mock.Create<AuthController>();

            ExceptionAssert.ThrowsAsync(new BadRequestException(ErrorCodes.LOGIN_FAILED,
                    new AuthenticateUnauthorizedResult
                    {
                        CaptchaRequiredUntil = captchaRequiredUntil,
                    }),
                () => sut.Authenticate(request));
        }

        [Fact]
        public void Should_ThrowException_When_PasswordIsInvalid()
        {
            using var mock = AutoMock.GetLoose();

            const string email = "email";
            const string token = "token";
            const string password = "password";
            var captchaRequiredUntil = new DateTime();

            var request = new AuthenticateRequest
            {
                Password = password,
                Email = email,
                CaptchaToken = token
            };

            var user = new User();

            mock.Mock<IUserManager>()
                .Setup(manager => manager.FindByEmailAsync(email))
                .ReturnsAsync(user);

            mock.Mock<IUserAccessFailedService>()
                .Setup(service => service.IsCaptchaRequired(user))
                .ReturnsAsync(captchaRequiredUntil);

            mock.Mock<ICaptchaValidationService>()
                .Setup(service => service.IsValidAsync(token))
                .ReturnsAsync(true);

            mock.Mock<IUserManager>()
                .Setup(manager => manager.CheckPasswordAsync(user, password))
                .ReturnsAsync(false);

            var sut = mock.Create<AuthController>();

            ExceptionAssert.ThrowsAsync(new BadRequestException(ErrorCodes.LOGIN_FAILED,
                    new AuthenticateUnauthorizedResult
                    {
                        CaptchaRequiredUntil = captchaRequiredUntil,
                    }),
                () => sut.Authenticate(request));

            mock.Mock<IUserAccessFailedService>()
                .Verify(service => service.RegisterFailedAttempt(user));
        }

        [Fact]
        public async Task Should_ReturnAuthenticationResult_When_EveryCaptchaVerificationIsPassedAndPasswordIsCorrect()
        {
            using var mock = AutoMock.GetLoose();

            const string email = "email";
            const string token = "token";
            const string password = "password";
            var captchaRequiredUntil = new DateTime();

            var request = new AuthenticateRequest
            {
                Password = password,
                Email = email,
                CaptchaToken = token
            };

            var user = new User();

            var expected = new AuthenticateResult
            {
                Token = "token"
            };

            mock.Mock<IUserManager>()
                .Setup(manager => manager.FindByEmailAsync(email))
                .ReturnsAsync(user);

            mock.Mock<IUserAccessFailedService>()
                .Setup(service => service.IsCaptchaRequired(user))
                .ReturnsAsync(captchaRequiredUntil);

            mock.Mock<ICaptchaValidationService>()
                .Setup(service => service.IsValidAsync(token))
                .ReturnsAsync(true);

            mock.Mock<IUserManager>()
                .Setup(manager => manager.CheckPasswordAsync(user, password))
                .ReturnsAsync(true);

            mock.Mock<IAuthenticationResultProvider>()
                .Setup(provider => provider.Get(user))
                .Returns(expected);

            var sut = mock.Create<AuthController>();

            var actual = await sut.Authenticate(request);

            ContentAssert.AreEqual(actual, expected);
        }
    }
}