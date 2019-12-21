using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectStandard.Extensions
{
  public static class XXString
  {
    public static string Left(this string value, int length)
    {
      if (string.IsNullOrEmpty(value)) return value;

      length = Math.Abs(length);

      return (value.Length <= length
             ? value
             : value.Substring(0, length)
             );
    }


    public static string Right(this string value, int length)
    {
      value = (value ?? string.Empty);

      length = Math.Abs(length);

      return (value.Length >= length)
          ? value.Substring(value.Length - length, length)
          : value;
    }


    public static string SurroundWithDoubleQuotes(this string text)
    {
      return SurroundWith(text, "\"");
    }

    public static string SurroundWith(this string text, string ends)
    {
      return ends + text + ends;
    }

  }
}
