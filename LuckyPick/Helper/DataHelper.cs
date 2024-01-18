using System;
using System.Linq;

namespace LuckyPick.Helper
{
    public class DataHelper
    {
        public static string SortDigit(string combination)
        {
            var combinationArr = combination.Replace(Environment.NewLine, string.Empty).Split('-');

            return string.Join("-", combinationArr.OrderBy(c => int.Parse(c)));
        }
    }
}
