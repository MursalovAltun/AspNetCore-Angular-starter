using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Application.Components.EmailSender
{
    public class SendGridEmailService : IEmailService
    {
        private readonly ISendGridClientContainer _sendGridClientContainer;
        private readonly EmailConfiguration _emailConfiguration;

        private ISendGridClient SendGridClient => _sendGridClientContainer.Get();

        public SendGridEmailService(
            IOptions<EmailConfiguration> emailConfiguration,
            ISendGridClientContainer sendGridClientContainer)
        {
            _sendGridClientContainer = sendGridClientContainer;
            _emailConfiguration = emailConfiguration.Value;
        }

        public async Task SendAsync(string email, string subject, string body)
        {
            await SendAsync(new SendSingleEmailRequest
            {
                Email = email,
                Subject = subject,
                Content = body
            });
        }

        public async Task SendAsync(SendSingleEmailRequest request)
        {
            var from = new EmailAddress(_emailConfiguration.SenderEmail, _emailConfiguration.SenderName);
            var to = new EmailAddress(request.Email);
            var content = string.IsNullOrEmpty(request.Content)
                ? " "
                : request.Content;
            var msg = MailHelper.CreateSingleEmail(from, to, request.Subject, content, content);

            if (request.Attachments != null && request.Attachments.Any())
            {
                msg.AddAttachments(ConvertToSendGridAttachments(request.Attachments));
            }

            await SendEmailAsync(msg);
        }

        public async Task SendAsync(SendMultipleEmailsRequest request)
        {
            var from = new EmailAddress(_emailConfiguration.SenderEmail, _emailConfiguration.SenderName);
            var tos = request.Emails.Select(email => new EmailAddress(email)).ToList();
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, request.Subject, request.Content,
                request.Content, true);

            if (request.Attachments != null && request.Attachments.Any())
            {
                msg.AddAttachments(ConvertToSendGridAttachments(request.Attachments));
            }

            await SendEmailAsync(msg);
        }

        private static List<Attachment> ConvertToSendGridAttachments(
            IEnumerable<SendEmailRequestAttachment> attachments)
        {
            return attachments.Select(a => new Attachment
                {
                    Type = a.Type,
                    Filename = a.Filename,
                    Content = a.Content,
                })
                .ToList();
        }

        private async Task SendEmailAsync(SendGridMessage msg)
        {
            msg.Personalizations.ForEach(p =>
            {
                if (p.Tos.Any(to => to.Email.ToLower().EndsWith("@employees.test")))
                {
                    var emails = p.Tos.Select(email => email.Email).Aggregate((a, b) => $"{a}, {b}");

                    p.Subject = $"{emails}: {p.Subject}";
                }
            });

            if (msg.Personalizations.Any(p => p.Tos.Any(to => to.Email.ToLower().EndsWith(".test"))))
            {
                return;
            }

            var response = await SendGridClient.SendEmailAsync(msg);

            if (response.StatusCode != HttpStatusCode.Accepted)
                throw new Exception(response.StatusCode.ToString());
        }
    }
}