using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Components.Captcha;
using Autofac;
using Autofac.Extras.Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

namespace Application.UnitTests.Components.Captcha
{
    public class CaptchaValidationServiceTests
    {
        [Fact]
        public async Task Should_ReturnTrue_When_RequestReturnsSuccessTrue()
        {
            const string verificationUrl = "http://localhost";
            const string secret = "secret";
            const string validationToken = "validationToken";
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var ip = ipAddress.ToString();

            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect(HttpMethod.Post, verificationUrl)
                .WithFormData(new[]
                {
                    new KeyValuePair<string, string>("secret", secret),
                    new KeyValuePair<string, string>("response", validationToken),
                    new KeyValuePair<string, string>("remoteip", ip)
                })
                .Respond("application/json", JsonConvert.SerializeObject(new {success = true}));

            var client = mockHttp.ToHttpClient();

            using var mock = AutoMock.GetLoose(cfg => { cfg.RegisterInstance(client).As<HttpClient>(); });

            mock.Mock<IOptions<CaptchaOptions>>()
                .SetupGet(configuration => configuration.Value)
                .Returns(new CaptchaOptions
                {
                    VerificationUrl = verificationUrl,
                    Secret = secret,
                });

            mock.Mock<IHttpContextAccessor>()
                .Setup(accessor => accessor.HttpContext.Connection.RemoteIpAddress)
                .Returns(ipAddress);

            var sut = mock.Create<CaptchaValidationService>();

            var actual = await sut.IsValidAsync(validationToken);

            Assert.True(actual);
        }

        [Fact]
        public async Task Should_ReturnFalse_When_RequestReturnsSuccessFalse()
        {
            const string verificationUrl = "http://localhost";
            const string secret = "secret";
            const string validationToken = "validationToken";
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var ip = ipAddress.ToString();

            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect(HttpMethod.Post, verificationUrl)
                .WithFormData(new[]
                {
                    new KeyValuePair<string, string>("secret", secret),
                    new KeyValuePair<string, string>("response", validationToken),
                    new KeyValuePair<string, string>("remoteip", ip)
                })
                .Respond("application/json", JsonConvert.SerializeObject(new {success = false}));

            var client = mockHttp.ToHttpClient();

            using var mock = AutoMock.GetLoose(cfg => { cfg.RegisterInstance(client).As<HttpClient>(); });

            mock.Mock<IOptions<CaptchaOptions>>()
                .SetupGet(configuration => configuration.Value)
                .Returns(new CaptchaOptions
                {
                    VerificationUrl = verificationUrl,
                    Secret = secret,
                });

            mock.Mock<IHttpContextAccessor>()
                .Setup(accessor => accessor.HttpContext.Connection.RemoteIpAddress)
                .Returns(ipAddress);

            var sut = mock.Create<CaptchaValidationService>();

            var actual = await sut.IsValidAsync(validationToken);

            Assert.False(actual);
        }
    }
}