using System.ComponentModel.DataAnnotations;

namespace Application.Components.PushNotifications
{
    public class PushNotificationsConfiguration
    {
        public bool UseMailHog { get; set; }
        [Required] public string PublicKey { get; set; }
        [Required] public string PrivateKey { get; set; }
    }
}