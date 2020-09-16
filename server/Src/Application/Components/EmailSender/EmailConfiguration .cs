using System.ComponentModel.DataAnnotations;
using Common;

namespace Application.Components.EmailSender
{
    public class EmailConfiguration
    {
        public bool UseMailHog { get; set; }

        [RequiredIf(nameof(UseMailHog), false)]
        public string ApiKey { get; set; }

        [Required] public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}