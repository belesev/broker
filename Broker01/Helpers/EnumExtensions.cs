using System;

namespace BrokerAlgo.Helpers
{
    public static class EnumExtensions
    {
        public static T Parse<T>(this string value) where T:struct
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
