namespace Application.Auth.Account
{
    public class AccountCreateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CaptchaToken { get; set; }
    }
}