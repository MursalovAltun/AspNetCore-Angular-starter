namespace Application.Auth.Webauthn
{
    public class WebauthnConfiguration
    {
        public string Origin { get; set; }
        public string ServerDomain { get; set; }
        public string ServerName { get; set; }
        public string MDSAccessKey { get; set; }
        public string MDSCacheDirPath { get; set; }
    }
}