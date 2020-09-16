using System.Collections.Generic;

namespace Application.Components.EmailSender
{
    public class SendMultipleEmailsRequest
    {
        public List<string> Emails { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public List<SendEmailRequestAttachment> Attachments { get; set; }
    }
}