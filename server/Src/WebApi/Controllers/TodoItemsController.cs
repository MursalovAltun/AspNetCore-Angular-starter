using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Components.TodoItems;
using Application.Providers.CurrentUserProvider;
using Application.TodoItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class TodoItemsController : Controller
    {
        private readonly ITodoItemsService _service;
        private readonly ICurrentUserProvider _currentUserProvider;

        public TodoItemsController(
            ITodoItemsService service,
            ICurrentUserProvider currentUserProvider)
        {
            _service = service;
            _currentUserProvider = currentUserProvider;
        }
        
        [HttpGet]
        public async Task<IEnumerable<TodoItemDto>> Get()
        {
            var user = await _currentUserProvider.GetUserAsync();
            
            return await _service.GetListAsync(user.Id);
        }

        [HttpPost]
        public async Task<TodoItemDto> Post([FromBody] TodoItemAddRequest request)
        {
            var user = await _currentUserProvider.GetUserAsync();

            return await _service.AddAsync(request, user.Id);
        }

        [HttpPut]
        public async Task<TodoItemDto> Put([FromBody] TodoItemDto request) => await _service.UpdateAsync(request);
        
        [HttpDelete]
        public async Task Delete([FromBody] TodoItemDeleteRequest request) => await _service.DeleteAsync(request);
    }
}