using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Components.TodoItems;
using Application.Profiles;
using Application.TodoItems;
using Application.UnitTests.Fixtures;
using Autofac.Extras.Moq;
using Common.Time;
using EF.Models;
using EF.Models.Models;
using UnitTests.Common.Asserts;
using Xunit;

namespace Application.UnitTests.Components.TodoItems
{
    public class TodoItemsServiceTests
    {
        private readonly TestFixture _fixture;
        private readonly AppDbContext _context;

        public TodoItemsServiceTests()
        {
            _fixture = new TestFixture(typeof(TodoItemProfile));
            _context = _fixture.Context;
        }

        [Fact]
        public async Task Should_ReturnTodoItems_For_UserIdInput()
        {
            using var mock = AutoMock.GetLoose(_fixture.BeforeBuild);

            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var itemId1 = Guid.NewGuid();
            var itemId2 = Guid.NewGuid();
            var itemId3 = Guid.NewGuid();

            _context.Add(new TodoItem
            {
                Id = itemId1,
                UserId = userId1
            });
            _context.Add(new TodoItem
            {
                Id = itemId2,
                UserId = userId2
            });
            _context.Add(new TodoItem
            {
                Id = itemId3,
                UserId = userId1
            });

            await _context.SaveChangesAsync();

            var sut = mock.Create<TodoItemsService>();

            var actual = await sut.GetListAsync(userId1);

            ContentAssert.AreEqual(actual,
                new List<TodoItemDto>
                {
                    new TodoItemDto {UserId = userId1, Id = itemId1},
                    new TodoItemDto {UserId = userId1, Id = itemId3}
                });
        }

        [Fact]
        public async Task Should_AddTodoItem_For_TodoItemDtoInput()
        {
            using var mock = AutoMock.GetLoose(_fixture.BeforeBuild);

            var userId = Guid.NewGuid();
            var lastModified = DateTime.Now;

            var request = new TodoItemAddRequest
            {
                Description = "description"
            };

            mock.Mock<IDateTimeProvider>()
                .Setup(provider => provider.UtcNow)
                .Returns(lastModified);

            var sut = mock.Create<TodoItemsService>();

            var result = await sut.AddAsync(request, userId);

            var added = _context.TodoItems.Single();

            var newItemId = added.Id;

            ContentAssert.AreEqual(added, new TodoItem
            {
                Id = newItemId,
                UserId = userId,
                Description = "description",
                Done = false,
                LastModified = lastModified,
            });

            ContentAssert.AreEqual(result, new TodoItemDto
            {
                Id = newItemId,
                UserId = userId,
                Description = "description",
                Done = false,
                LastModified = lastModified,
            });
        }

        [Fact]
        public async Task Should_UpdateDoneOfTodoItem_For_TodoItemUpdateDoneRequestInput()
        {
            using var mock = AutoMock.GetLoose(_fixture.BeforeBuild);

            var todoItemId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var lastModified = DateTime.Now;

            var request = new TodoItemUpdateDoneRequest
            {
                TodoItemId = todoItemId,
                Done = true
            };

            var todoItem = new TodoItem
            {
                Id = todoItemId,
                UserId = userId,
                Description = "description",
                Done = false,
                LastModified = new DateTime(),
            };

            _context.TodoItems.Add(todoItem);

            _context.SaveChanges();

            mock.Mock<IDateTimeProvider>()
                .Setup(provider => provider.UtcNow)
                .Returns(lastModified);

            var sut = mock.Create<TodoItemsService>();

            var result = await sut.DoneAsync(request);

            var updated = _context.TodoItems.Single();

            ContentAssert.AreEqual(updated, new TodoItem
            {
                Id = todoItemId,
                UserId = userId,
                Description = "description",
                Done = true,
                LastModified = lastModified,
            });

            ContentAssert.AreEqual(result, new TodoItemDto
            {
                Id = todoItemId,
                UserId = userId,
                Description = "description",
                Done = true,
                LastModified = lastModified,
            });
        }
    }
}