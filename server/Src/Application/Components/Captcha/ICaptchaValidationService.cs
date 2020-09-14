using System.Threading.Tasks;

namespace Application.Components.Captcha
{
    public interface ICaptchaValidationService
    {
        Task<bool> IsValidAsync(string validationToken);
    }
}