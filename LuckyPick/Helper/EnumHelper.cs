using LuckyPick.Enum;
using System;

namespace LuckyPick.Helper
{
    public class EnumHelper
    {
        public static string GetName<T>(object value)
        {
            return System.Enum.GetName(typeof(T), value);

        }
    }
}
