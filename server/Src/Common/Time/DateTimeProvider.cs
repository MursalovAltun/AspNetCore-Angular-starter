using System;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;

namespace Common.Time
{
    [As(typeof(IDateTimeProvider))]
    internal class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}