using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuckyPick.Services
{
    public static class CombinationHelper
    {
        public static IEnumerable<IEnumerable<T>> DifferentCombinations<T>(this IEnumerable<T> elements, int k)
        {
            if (k == 0)
            {
                return new[] { new T[0] };
            }
            else
            {
                var cons = elements.SelectMany((e, i) =>
                 elements.Skip(i + 1).DifferentCombinations(k - 1).Select(c => (new[] { e }).Concat(c)));

                foreach (var n in cons)
                {
                    if (n.Count() == 6)
                    {
                        LuckyPickService.FindSameDigit("SameDigit2", string.Join("-", n));
                    }
                }

                return cons;

            }
        }
        // Linq
        public static IEnumerable<IEnumerable<T>> GetDifferentCombinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).DifferentCombinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }
    }
}
