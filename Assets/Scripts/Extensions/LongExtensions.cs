using System;
using System.Globalization;

namespace Extensions
{
    public static class LongExtensions
    {
        public static string ToCurrency(this long val)
        {
            return String.Format("{0:n0}", val);
        }
    }
}