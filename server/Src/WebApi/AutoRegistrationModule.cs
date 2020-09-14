using Autofac;
using Autofac.Extras.RegistrationAttributes;

namespace WebApi
{
    public class AutoRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AutoRegistration(GetType().Assembly);
            builder.RegisterModule<Application.AutoRegistrationModule>();
        }
    }
}