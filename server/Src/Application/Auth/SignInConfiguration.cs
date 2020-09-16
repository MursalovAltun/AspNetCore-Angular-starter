using System.ComponentModel.DataAnnotations;

namespace Application.Auth
{
    public class SignInConfiguration
    {
        [Required] public byte? FailedAttemptsCount { get; set; }

        [Required] public byte? CaptchaLockoutMinutes { get; set; }
    }
}