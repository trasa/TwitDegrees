using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitDegrees.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FormatWith(this string format, params object[] args)
        {
            return String.Format(format, args);
        }

        public static byte[] GetBytes(this string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        public static string ToBase64String(this byte[] input)
        {
            return Convert.ToBase64String(input);
        }
    }
}
