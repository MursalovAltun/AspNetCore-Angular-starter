using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Application.Components.EmailSender
{
    public class MailHogEmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public MailHogEmailService(
            IOptions<EmailConfiguration> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
        }

        public async Task SendAsync(string email, string subject, string body)
        {
            await SendAsync(
                new SendSingleEmailRequest
                {
                    Email = email,
                    Subject = subject,
                    Content = body
                });
        }

        public async Task SendAsync(SendSingleEmailRequest request)
        {
            var from = new MailAddress(_emailConfiguration.SenderEmail, _emailConfiguration.SenderName);
            var to = new MailAddress(request.Email);
            var content = string.IsNullOrEmpty(request.Content)
                ? " "
                : request.Content;

            var message = new MailMessage(from, to) {Subject = request.Subject, Body = content};

            if (request.Attachments != null && request.Attachments.Any())
            {
                foreach (var attachment in ConvertToSendGridAttachments(request.Attachments))
                {
                    message.Attachments.Add(attachment);
                }
            }

            await SendEmailAsync(message);

            foreach (var attachment in message.Attachments)
            {
                attachment.ContentStream.Close();
            }
        }

        private async Task SendEmailAsync(MailMessage message)
        {
            var client = new SmtpClient("localhost", 1025);
            await client.SendMailAsync(message);
        }

        private static IEnumerable<Attachment> ConvertToSendGridAttachments(
            IEnumerable<SendEmailRequestAttachment> attachments)
        {
            return attachments
                .Select(a => new Attachment(
                        new MemoryStream(Convert.FromBase64String(a.Content)),
                        new ContentType
                        {
                            MediaType = a.Type.Contains("calendar")
                                ? "text/plain"
                                : a.Type,
                            Name = a.Filename
                        }
                    )
                ).ToList();
        }

        public async Task SendAsync(SendMultipleEmailsRequest request)
        {
            var from = new MailAddress(_emailConfiguration.SenderEmail, _emailConfiguration.SenderName);
            var tos = request.Emails.Select(email => new MailAddress(email));
            var content = string.IsNullOrEmpty(request.Content)
                ? " "
                : request.Content;

            var message = new MailMessage {Subject = request.Subject, Body = content, From = from};

            foreach (var to in tos)
            {
                message.To.Add(to);
            }

            if (request.Attachments != null && request.Attachments.Any())
            {
                foreach (var attachment in ConvertToSendGridAttachments(request.Attachments))
                {
                    message.Attachments.Add(attachment);
                }
            }

            await SendEmailAsync(message);

            foreach (var attachment in message.Attachments)
            {
                attachment.ContentStream.Close();
            }
        }
    }
}