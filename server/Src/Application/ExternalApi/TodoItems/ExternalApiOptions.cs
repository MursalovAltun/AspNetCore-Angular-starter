using System.ComponentModel.DataAnnotations;

namespace Application.ExternalApi.TodoItems
{
    public class ExternalTodoItemsApiOptions
    {
        [Required] public string Host { get; set; }
    }
}