using System;
using System.Security.Claims;
using Application.Providers.CurrentUserProvider;
using Autofac.Extras.Moq;
using EF.Models.Models;
using Microsoft.AspNetCore.Http;
using Moq;

namespace UnitTests.Common.Extensions
{
    public static class AutoMockExtensions
    {
        public static ClaimsPrincipal GetClaimsPrincipal(this AutoMock autoMock)
        {
            var claimsPrincipal = autoMock.Mock<ClaimsPrincipal>().Object;

            var httpContextMock = autoMock.Mock<HttpContext>();

            autoMock.Mock<IHttpContextAccessor>()
                .SetupGet(accessor => accessor.HttpContext)
                .Returns(httpContextMock.Object);

            httpContextMock
                .SetupGet(context => context.User)
                .Returns(claimsPrincipal);

            return claimsPrincipal;
        }

        public static ClaimsPrincipal SetCurrentUser(this AutoMock autoMock, User user,
            ClaimsPrincipal claimsPrincipal = null)
        {
            claimsPrincipal ??= autoMock.GetClaimsPrincipal();

            autoMock.Mock<ICurrentUserProvider>()
                .Setup(provider => provider.GetUserAsync())
                .ReturnsAsync(user);

            return claimsPrincipal;
        }

        public static ClaimsPrincipal SetCurrentUser(this AutoMock autoMock, Guid userId,
            ClaimsPrincipal claimsPrincipal = null)
        {
            claimsPrincipal ??= autoMock.GetClaimsPrincipal();

            autoMock.Mock<ICurrentUserProvider>()
                .Setup(provider => provider.GetUserAsync())
                .ReturnsAsync(new User {Id = userId});

            return claimsPrincipal;
        }
    }
}