using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jammy.Helpers
{
    public static class NumberHelpers
    {
        public static int Clamp (int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;

            return value;
        }

        public static int LowerClamp (int value, int min)
        {
            if (value < min)
                return min;

            return value;
        }

        public static int NextDouble2 (this Random self)
        {
            return self.NextDouble() < 0.5 ? 0 : 1;
        }
    }
}
