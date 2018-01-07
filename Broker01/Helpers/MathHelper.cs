using System;
using System.Collections.Generic;
using System.Linq;

namespace BrokerAlgo.Helpers
{
    public static class MathHelper
    {
        public static decimal AddPercent(this decimal value, decimal percent)
        {
            if (percent <= 0)
                throw new ArgumentOutOfRangeException(nameof(percent));
            return value * (1 + percent/100m);
        }

        public static decimal SubtractPercent(this decimal value, decimal percent)
        {
            if (percent <= 0)
                throw new ArgumentOutOfRangeException(nameof(percent));
            return value * (1 - percent / 100m);
        }

        public static decimal Percentile(IEnumerable<decimal> seq, decimal percentile)
        {
            var elements = seq.ToArray();
            Array.Sort(elements);
            decimal realIndex = percentile * (elements.Length - 1);
            int index = (int)realIndex;
            decimal frac = realIndex - index;
            if (index + 1 < elements.Length)
                return elements[index] * (1 - frac) + elements[index + 1] * frac;
            else
                return elements[index];
        }
    }
}
