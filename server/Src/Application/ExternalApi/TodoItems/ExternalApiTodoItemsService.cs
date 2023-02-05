using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.Components.TodoItems;
using Application.TodoItems;
using Common.GuidProvider;
using Common.Time;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Application.ExternalApi.TodoItems
{
    public class ExternalApiTodoItemsService : ITodoItemsService
    {
        private readonly HttpClient _client;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IGuidProvider _guidProvider;

        public ExternalApiTodoItemsService(HttpClient client,
            IDateTimeProvider dateTimeProvider,
            IGuidProvider guidProvider)
        {
            _client = client;
            _dateTimeProvider = dateTimeProvider;
            _guidProvider = guidProvider;
        }

        public async Task<IEnumerable<TodoItemDto>> GetListAsync(Guid userId)
        {
            var httpResult = await _client.GetAsync($"/todo-items?userId={userId}");

            if (!httpResult.IsSuccessStatusCode) throw new Exception();

            var content = await httpResult.Content.ReadAsStringAsync();

            var jArray = (JArray)JsonConvert.DeserializeObject(content);

            return jArray.ToObject<List<TodoItemDto>>();
        }

        private async Task<TodoItemDto> GetAsync(Guid id)
        {
            var httpResult = await _client.GetAsync($"/todo-items/{id}");

            if (!httpResult.IsSuccessStatusCode) throw new Exception();

            var content = await httpResult.Content.ReadAsStringAsync();

            var jObject = (JObject)JsonConvert.DeserializeObject(content);

            return jObject.ToObject<TodoItemDto>();
        }

        public async Task<TodoItemDto> AddAsync(TodoItemAddRequest request, Guid userId)
        {
            var todoItemDto = new TodoItemDto
            {
                Description = request.Description,
                Id = _guidProvider.NewGuid(),
                UserId = userId,
                Done = false,
                LastModified = _dateTimeProvider.UtcNow
            };
            var content = new StringContent(JsonConvert.SerializeObject(todoItemDto, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }),
                Encoding.UTF8, "application/json");
            var httpResult = await _client.PostAsync("/todo-items", content);

            if (!httpResult.IsSuccessStatusCode) throw new Exception();

            return todoItemDto;
        }

        public async Task<TodoItemDto> DoneAsync(TodoItemUpdateDoneRequest request)
        {
            var todoItemDto = await GetAsync(request.TodoItemId);
            todoItemDto.Done = request.Done;
            todoItemDto.LastModified = _dateTimeProvider.UtcNow;
            var content = new StringContent(JsonConvert.SerializeObject(todoItemDto, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }),
                Encoding.UTF8, "application/json");
            var httpResult = await _client.PutAsync($"/todo-items/{request.TodoItemId}", content);

            if (!httpResult.IsSuccessStatusCode) throw new Exception();

            return todoItemDto;
        }

        public async Task<TodoItemDto> UpdateAsync(TodoItemDto request)
        {
            var todoItemDto = await GetAsync(request.Id);
            todoItemDto.Description = request.Description;
            todoItemDto.LastModified = _dateTimeProvider.UtcNow;
            var content = new StringContent(JsonConvert.SerializeObject(todoItemDto, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }),
                Encoding.UTF8, "application/json");
            var httpResult = await _client.PutAsync($"/todo-items/{request.Id}", content);

            if (!httpResult.IsSuccessStatusCode) throw new Exception();

            return todoItemDto;
        }

        public Task DeleteAsync(TodoItemDeleteRequest request)
        {
            throw new NotImplementedException();
        }
    }
}