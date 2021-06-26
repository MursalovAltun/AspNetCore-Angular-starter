using System.ComponentModel.DataAnnotations;

namespace Application.Components.Captcha
{
    public class CaptchaOptions
    {
        [Required] public string Secret { get; set; }
        [Required] public string ClientKey { get; set; }
        [Required] public string VerificationUrl { get; set; }
        public bool Enabled { get; set; }
    }
}