using SendGrid;

namespace Application.Components.EmailSender
{
    public interface ISendGridClientContainer
    {
        ISendGridClient Get();
    }
}