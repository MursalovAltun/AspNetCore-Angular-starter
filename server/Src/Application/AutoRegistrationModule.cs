using Autofac;
using Autofac.Extras.RegistrationAttributes;

namespace Application
{
    public class AutoRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AutoRegistration(GetType().Assembly);
            builder.RegisterModule<Common.AutoRegistrationModule>();
        }
    }
}