using System;

namespace Common
{
    public static class EnumExtensions
    {
        public static string ToStringName<TEnum>(this TEnum key)
        {
            return Enum.GetName(typeof(TEnum), key);
        }
    }
}