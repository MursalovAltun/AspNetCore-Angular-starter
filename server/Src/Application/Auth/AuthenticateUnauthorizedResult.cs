using System;

namespace Application.Auth
{
    public class AuthenticateUnauthorizedResult
    {
        public DateTime? CaptchaRequiredUntil { get; set; }
    }
}
