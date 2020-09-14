namespace Application.Components.Captcha
{
    public class CaptchaOptions
    {
        public string Secret { get; set; }
        public string ClientKey { get; set; }
        public string VerificationUrl { get; set; }
    }
}