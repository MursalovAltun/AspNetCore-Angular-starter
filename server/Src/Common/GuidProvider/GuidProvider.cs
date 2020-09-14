using System;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;

namespace Common.GuidProvider
{
    [As(typeof(IGuidProvider))]
    internal class GuidProvider : IGuidProvider
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}