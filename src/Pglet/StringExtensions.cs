using System;
using System.Collections.Generic;
using System.Text;

namespace Pglet
{
    public static class StringExtensions
    {
        public static string Encode(this string str)
        {
            return str != null ? str.Replace("\\", "\\\\").Replace("\n", "\\n").Replace("\"", "\\\"") : str;
        }
    }
}
