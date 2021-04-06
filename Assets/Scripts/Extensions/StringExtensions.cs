using System;
using System.Globalization;

namespace Extensions
{
    public static class StringExtensions
    {
        public static string ToCurrency(this string val)
        {
            if (String.IsNullOrWhiteSpace(val))
                return string.Empty;
            return String.Format("{0:n0}", val);
        }
    }
}