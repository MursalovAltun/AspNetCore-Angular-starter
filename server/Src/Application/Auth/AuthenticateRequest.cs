namespace Application.Auth
{
    public class AuthenticateRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CaptchaToken { get; set; }
    }
}