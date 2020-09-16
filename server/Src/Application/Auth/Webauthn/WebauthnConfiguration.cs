using System.ComponentModel.DataAnnotations;

namespace Application.Auth.Webauthn
{
    public class WebauthnConfiguration
    {
        [Required] public string Origin { get; set; }
        [Required] public string ServerDomain { get; set; }
        [Required] public string ServerName { get; set; }
        public string MDSAccessKey { get; set; }
        public string MDSCacheDirPath { get; set; }
    }
}