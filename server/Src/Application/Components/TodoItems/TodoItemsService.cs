using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.TodoItems;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Time;
using EF.Models;
using EF.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Components.TodoItems
{
    [As(typeof(ITodoItemsService))]
    public class TodoItemsService : ITodoItemsService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITodoItemsPushNotificationService _todoItemsPushNotificationService;

        public TodoItemsService(AppDbContext context,
            IMapper mapper,
            IDateTimeProvider dateTimeProvider,
            ITodoItemsPushNotificationService todoItemsPushNotificationService)
        {
            _context = context;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _todoItemsPushNotificationService = todoItemsPushNotificationService;
        }

        public async Task<IEnumerable<TodoItemDto>> GetListAsync(Guid userId)
        {
            return await _context.TodoItems
                .Where(todoItem => todoItem.UserId == userId)
                .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TodoItemDto> AddAsync(TodoItemAddRequest request, Guid userId)
        {
            var todoItem = new TodoItem
            {
                Description = request.Description,
                UserId = userId,
                Done = false,
                LastModified = _dateTimeProvider.UtcNow,
            };

            _context.TodoItems.Add(todoItem);

            await _context.SaveChangesAsync();

            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task<TodoItemDto> DoneAsync(TodoItemUpdateDoneRequest request)
        {
            var todoItem = await _context.TodoItems.FindAsync(request.TodoItemId);

            todoItem.Done = request.Done;
            todoItem.LastModified = _dateTimeProvider.UtcNow;

            _context.TodoItems.Update(todoItem);

            await _context.SaveChangesAsync();

            if (todoItem.Done)
            {
                await _todoItemsPushNotificationService.SendAsync(todoItem);
            }

            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task<TodoItemDto> UpdateAsync(TodoItemDto request)
        {
            var todoItem = await _context.TodoItems.FindAsync(request.Id);

            todoItem.Description = request.Description;
            todoItem.LastModified = _dateTimeProvider.UtcNow;

            _context.TodoItems.Update(todoItem);

            await _context.SaveChangesAsync();

            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task DeleteAsync(TodoItemDeleteRequest request)
        {
            var item = await _context.TodoItems.FindAsync(request.Id);

            _context.Remove(item);
        }
    }
}