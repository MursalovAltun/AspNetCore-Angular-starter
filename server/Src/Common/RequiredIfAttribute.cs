using System;
using System.ComponentModel.DataAnnotations;

namespace Common
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class RequiredIfAttribute : RequiredAttribute
    {
        private string PropertyName { get; }
        private object DesiredValue { get; }

        public RequiredIfAttribute(string propertyName, object desiredValue)
        {
            PropertyName = propertyName;
            DesiredValue = desiredValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();
            var propertyValue = type.GetProperty(PropertyName)?.GetValue(instance, null);
            if (propertyValue == null || propertyValue.ToString() != DesiredValue.ToString())
                return ValidationResult.Success;
            var result = base.IsValid(value, context);
            return result;
        }
    }
}