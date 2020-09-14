namespace Application.Components.PushNotifications
{
    public class PushSubscriptionDto
    {
        public string Endpoint { get; set; }

        public string Auth { get; set; }

        public string P256DH { get; set; }
    }
}