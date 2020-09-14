namespace Application.Components.PushNotifications
{
    public interface INotificationOptionsToPayloadConverter
    {
        string Convert(PushNotificationOptions notificationOptions);
    }
}