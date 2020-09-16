using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Components.TodoItems;
using Application.ExternalApi.TodoItems;
using Application.TodoItems;
using Autofac;
using Autofac.Extras.Moq;
using Common.GuidProvider;
using Common.Time;
using EF.Models.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RichardSzalay.MockHttp;
using UnitTests.Common.Asserts;
using Xunit;

namespace Application.UnitTests.Components.ExternalApi
{
    public class ExternalApiTodoItemsServiceTests
    {
        [Fact]
        public async Task Should_ReturnTodoItems_For_UserIdInput_When_RequestSucceed()
        {
            var userId1 = Guid.NewGuid();
            var itemId1 = Guid.NewGuid();
            var itemId3 = Guid.NewGuid();

            var items = new List<TodoItem>
            {
                new TodoItem
                {
                    Id = itemId1,
                    UserId = userId1
                },
                new TodoItem
                {
                    Id = itemId3,
                    UserId = userId1
                }
            };

            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect($"http://localhost/todo-items?userId={userId1}")
                .Respond("application/json", JsonConvert.SerializeObject(items, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost");

            using var mock = AutoMock.GetLoose(cfg => { cfg.RegisterInstance(client).As<HttpClient>(); });

            var sut = mock.Create<ExternalApiTodoItemsService>();

            var actual = await sut.GetListAsync(userId1);

            ContentAssert.AreEqual(actual,
                new List<TodoItemDto>
                {
                    new TodoItemDto {UserId = userId1, Id = itemId1},
                    new TodoItemDto {UserId = userId1, Id = itemId3}
                });
        }

        [Fact]
        public void Should_ThrowException_For_UserIdInput_When_RequestFailed()
        {
            var userId = Guid.NewGuid();

            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect($"http://localhost/todo-items?userId={userId}")
                .Respond(HttpStatusCode.BadRequest);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost");

            using var mock = AutoMock.GetLoose(cfg => { cfg.RegisterInstance(client).As<HttpClient>(); });

            var sut = mock.Create<ExternalApiTodoItemsService>();

            ExceptionAssert.ThrowsAsync<Exception>(() => sut.GetListAsync(userId));
        }

        [Fact]
        public async Task Should_AddTodoItem_For_TodoItemDtoInput_When_RequestSucceed()
        {
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var lastModified = DateTime.Now;

            var todoItemDto = new TodoItemDto
            {
                Description = "description",
                Id = itemId,
                UserId = userId,
                Done = false,
                LastModified = lastModified
            };
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect(HttpMethod.Post, "http://localhost/todo-items")
                .WithContent(JsonConvert.SerializeObject(todoItemDto, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }))
                .Respond(HttpStatusCode.Accepted);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost");

            using var mock = AutoMock.GetLoose(cfg => { cfg.RegisterInstance(client).As<HttpClient>(); });

            var request = new TodoItemAddRequest
            {
                Description = "description"
            };

            mock.Mock<IDateTimeProvider>()
                .Setup(provider => provider.UtcNow)
                .Returns(lastModified);

            mock.Mock<IGuidProvider>()
                .Setup(provider => provider.NewGuid())
                .Returns(itemId);

            var sut = mock.Create<ExternalApiTodoItemsService>();

            var result = await sut.AddAsync(request, userId);

            ContentAssert.AreEqual(result, new TodoItemDto
            {
                Id = itemId,
                UserId = userId,
                Description = "description",
                Done = false,
                LastModified = lastModified,
            });
        }

        [Fact]
        public void Should_ThrowException_For_TodoItemDtoInput_When_RequestFailed()
        {
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var lastModified = DateTime.Now;

            var todoItemDto = new TodoItemDto
            {
                Description = "description",
                Id = itemId,
                UserId = userId,
                Done = false,
                LastModified = lastModified
            };
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect(HttpMethod.Post, "http://localhost/todo-items")
                .WithContent(JsonConvert.SerializeObject(todoItemDto, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }))
                .Respond(HttpStatusCode.BadRequest);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost");

            using var mock = AutoMock.GetLoose(cfg => { cfg.RegisterInstance(client).As<HttpClient>(); });

            var request = new TodoItemAddRequest
            {
                Description = "description"
            };

            mock.Mock<IDateTimeProvider>()
                .Setup(provider => provider.UtcNow)
                .Returns(lastModified);

            mock.Mock<IGuidProvider>()
                .Setup(provider => provider.NewGuid())
                .Returns(itemId);

            var sut = mock.Create<ExternalApiTodoItemsService>();

            ExceptionAssert.ThrowsAsync<Exception>(() => sut.AddAsync(request, userId));
        }

        [Fact]
        public async Task Should_UpdateDoneOfTodoItem_For_TodoItemUpdateDoneRequestInput_When_RequestSucceed()
        {
            var todoItemId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var lastModified = DateTime.Now;

            var request = new TodoItemUpdateDoneRequest
            {
                TodoItemId = todoItemId,
                Done = true
            };

            var todoItemDto = new TodoItemDto
            {
                Id = todoItemId,
                UserId = userId,
                Description = "description",
                Done = false,
                LastModified = new DateTime(),
            };
            var updatedTodoItemDto = new TodoItemDto
            {
                Id = todoItemId,
                UserId = userId,
                Description = "description",
                Done = true,
                LastModified = lastModified,
            };

            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect($"http://localhost/todo-items/{todoItemId}")
                .Respond("application/json", JsonConvert.SerializeObject(todoItemDto, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
            mockHttp
                .Expect(HttpMethod.Put, $"http://localhost/todo-items/{todoItemId}")
                .WithContent(JsonConvert.SerializeObject(updatedTodoItemDto, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }))
                .Respond(HttpStatusCode.Accepted);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost");

            using var mock = AutoMock.GetLoose(cfg => { cfg.RegisterInstance(client).As<HttpClient>(); });

            mock.Mock<IDateTimeProvider>()
                .Setup(provider => provider.UtcNow)
                .Returns(lastModified);

            var sut = mock.Create<ExternalApiTodoItemsService>();

            var result = await sut.DoneAsync(request);

            ContentAssert.AreEqual(result, new TodoItemDto
            {
                Id = todoItemId,
                UserId = userId,
                Description = "description",
                Done = true,
                LastModified = lastModified,
            });
        }

        [Fact]
        public void Should_UpdateDoneOfTodoItem_For_TodoItemUpdateDoneRequestInput_When_GetRequestFailed()
        {
            var todoItemId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var lastModified = DateTime.Now;

            var request = new TodoItemUpdateDoneRequest
            {
                TodoItemId = todoItemId,
                Done = true
            };

            var todoItemDto = new TodoItemDto
            {
                Id = todoItemId,
                UserId = userId,
                Description = "description",
                Done = false,
                LastModified = new DateTime(),
            };
            var updatedTodoItemDto = new TodoItemDto
            {
                Id = todoItemId,
                UserId = userId,
                Description = "description",
                Done = true,
                LastModified = lastModified,
            };

            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect($"http://localhost/todo-items/{todoItemId}")
                .Respond("application/json", JsonConvert.SerializeObject(todoItemDto, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
            mockHttp
                .Expect(HttpMethod.Put, $"http://localhost/todo-items/{todoItemId}")
                .WithContent(JsonConvert.SerializeObject(updatedTodoItemDto, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }))
                .Respond(HttpStatusCode.BadRequest);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost");

            using var mock = AutoMock.GetLoose(cfg => { cfg.RegisterInstance(client).As<HttpClient>(); });

            mock.Mock<IDateTimeProvider>()
                .Setup(provider => provider.UtcNow)
                .Returns(lastModified);

            var sut = mock.Create<ExternalApiTodoItemsService>();

            ExceptionAssert.ThrowsAsync<Exception>(() => sut.DoneAsync(request));
        }

        [Fact]
        public void Should_UpdateDoneOfTodoItem_For_TodoItemUpdateDoneRequestInput_When_PutRequestFailed()
        {
            var todoItemId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var lastModified = DateTime.Now;

            var request = new TodoItemUpdateDoneRequest
            {
                TodoItemId = todoItemId,
                Done = true
            };

            var updatedTodoItemDto = new TodoItemDto
            {
                Id = todoItemId,
                UserId = userId,
                Description = "description",
                Done = true,
                LastModified = lastModified,
            };

            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .Expect($"http://localhost/todo-items/{todoItemId}")
                .Respond(HttpStatusCode.BadRequest);
            mockHttp
                .Expect(HttpMethod.Put, $"http://localhost/todo-items/{todoItemId}")
                .WithContent(JsonConvert.SerializeObject(updatedTodoItemDto, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }))
                .Respond(HttpStatusCode.Accepted);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost");

            using var mock = AutoMock.GetLoose(cfg => { cfg.RegisterInstance(client).As<HttpClient>(); });

            mock.Mock<IDateTimeProvider>()
                .Setup(provider => provider.UtcNow)
                .Returns(lastModified);

            var sut = mock.Create<ExternalApiTodoItemsService>();

            ExceptionAssert.ThrowsAsync<Exception>(() => sut.DoneAsync(request));
        }
    }
}