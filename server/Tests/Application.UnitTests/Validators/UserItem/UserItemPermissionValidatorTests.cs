using System;
using System.Threading.Tasks;
using Application.Providers.CurrentUserProvider;
using Application.UnitTests.Fixtures;
using Application.Validators.UserItem;
using Autofac.Extras.Moq;
using EF.Models;
using EF.Models.Models;
using Moq;
using Xunit;

namespace Application.UnitTests.Validators.UserItem
{
    public class UserItemPermissionValidatorTests
    {
        private readonly TestFixture _fixture;
        private readonly AppDbContext _context;

        public UserItemPermissionValidatorTests()
        {
            _fixture = new TestFixture();
            _context = _fixture.Context;
        }

        [Fact]
        public async Task Should_ReturnTrue_When_ItemIsRelatedToTheCurrentUser()
        {
            using var mock = AutoMock.GetLoose(_fixture.BeforeBuild);

            var todoItemId = Guid.NewGuid();

            var user = new User();

            _context.TodoItems.Add(new TodoItem
            {
                Id = todoItemId,
                User = user,
            });

            await _context.SaveChangesAsync();

            mock.Mock<ICurrentUserProvider>()
                .Setup(provider => provider.GetUserAsync())
                .ReturnsAsync(user);

            var sut = mock.Create<UserItemPermissionValidator<TodoItem>>();

            var actual = await sut.HasAccess(todoItemId);

            Assert.True(actual);
        }

        [Fact]
        public async Task Should_ReturnFalse_When_ItemIsNotRelatedToTheCurrentUser()
        {
            using var mock = AutoMock.GetLoose(_fixture.BeforeBuild);

            var todoItemId = Guid.NewGuid();

            var user = new User();
            var user1 = new User();

            _context.TodoItems.Add(new TodoItem
            {
                Id = todoItemId,
                User = user,
            });

            _context.Users.Add(user1);

            await _context.SaveChangesAsync();

            mock.Mock<ICurrentUserProvider>()
                .Setup(provider => provider.GetUserAsync())
                .ReturnsAsync(user1);

            var sut = mock.Create<UserItemPermissionValidator<TodoItem>>();

            var actual = await sut.HasAccess(todoItemId);

            Assert.False(actual);
        }
    }
}