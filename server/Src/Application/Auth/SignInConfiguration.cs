namespace Application.Auth
{
    public class SignInConfiguration
    {
        public byte FailedAttemptsCount { get; set; }

        public byte CaptchaLockoutMinutes { get; set; }
    }
}