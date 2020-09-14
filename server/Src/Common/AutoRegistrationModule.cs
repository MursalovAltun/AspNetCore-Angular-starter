using Autofac;
using Autofac.Extras.RegistrationAttributes;

namespace Common
{
    public class AutoRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AutoRegistration(GetType().Assembly);
        }
    }
}