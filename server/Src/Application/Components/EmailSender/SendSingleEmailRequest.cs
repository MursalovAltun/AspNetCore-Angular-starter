using System.Collections.Generic;

namespace Application.Components.EmailSender
{
    public class SendSingleEmailRequest
    {
        public string Email { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public List<SendEmailRequestAttachment> Attachments { get; set; }
    }
}