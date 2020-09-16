using System.ComponentModel.DataAnnotations;

namespace Application
{
    public class CommonConfiguration
    {
        [Required] public string ClientBaseUrl { get; set; }
    }
}