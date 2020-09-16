using System.Threading.Tasks;

namespace Application.Components.EmailSender
{
    public interface IEmailService
    {
        Task SendAsync(SendSingleEmailRequest request);

        Task SendAsync(SendMultipleEmailsRequest request);

        Task SendAsync(string email, string subject, string body);
    }
}
