using Application.TodoItems;
using AutoMapper;
using EF.Models.Models;

namespace Application.Profiles
{
    public class TodoItemProfile : Profile
    {
        public TodoItemProfile()
        {
            CreateMap<TodoItem, TodoItemDto>();
        }
    }
}