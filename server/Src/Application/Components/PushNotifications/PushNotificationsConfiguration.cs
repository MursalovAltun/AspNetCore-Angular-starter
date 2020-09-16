using System.ComponentModel.DataAnnotations;

namespace Application.Components.PushNotifications
{
    public class PushNotificationsConfiguration
    {
        [Required] public string PublicKey { get; set; }
        [Required] public string PrivateKey { get; set; }
    }
}