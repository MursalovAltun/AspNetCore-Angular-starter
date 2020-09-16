using Microsoft.Extensions.Options;
using SendGrid;

namespace Application.Components.EmailSender
{
    public class SendGridClientContainer : ISendGridClientContainer
    {
        private readonly IOptions<EmailConfiguration> _emailConfiguration;
        private ISendGridClient _sendGridClient;

        public SendGridClientContainer(IOptions<EmailConfiguration> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public ISendGridClient Get()
        {
            return _sendGridClient ??= new SendGridClient(_emailConfiguration.Value.ApiKey);
        }
    }
}