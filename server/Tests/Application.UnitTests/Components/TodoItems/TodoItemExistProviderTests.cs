using System;
using System.Threading.Tasks;
using Application.Components.TodoItems;
using Application.UnitTests.Fixtures;
using Autofac.Extras.Moq;
using EF.Models;
using EF.Models.Models;
using UnitTests.Common.Extensions;
using Xunit;

namespace Application.UnitTests.Components.TodoItems
{
    public class TodoItemExistProviderTests
    {
        private readonly TestFixture _fixture;
        private readonly AppDbContext _context;

        public TodoItemExistProviderTests()
        {
            _fixture = new TestFixture();
            _context = _fixture.Context;
        }

        [Fact]
        public async Task Should_Return_True_When_Item_Exists()
        {
            using var mock = AutoMock.GetLoose(_fixture.BeforeBuild);

            const string description = "description";
            var userId = Guid.NewGuid();

            var todo = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = description,
                UserId = userId
            };

            _context.Add(todo);
            _context.SaveChanges();

            mock.SetCurrentUser(userId);

            var sut = mock.Create<TodoItemExistProvider>();
            
            var actual = await sut.ExistAsync(description);

            Assert.True(actual);
        }

        [Fact]
        public async Task Should_Return_False_When_Item_Not_Exists()
        {
            using var mock = AutoMock.GetLoose(_fixture.BeforeBuild);

            const string description = "description";
            var userId = Guid.NewGuid();

            mock.SetCurrentUser(userId);

            var sut = mock.Create<TodoItemExistProvider>();
            
            var actual = await sut.ExistAsync(description);

            Assert.False(actual);
        }
    }
}