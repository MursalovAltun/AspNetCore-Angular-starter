using Autofac;
using Autofac.Extras.RegistrationAttributes;

namespace EF.Models
{
    public class AutoRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AutoRegistration(GetType().Assembly);
        }
    }
}