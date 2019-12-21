using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectStandard.Tools
{
  public class TTString
  {
    public static bool IsHexOnly(string StringValue)
    {
      // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
      return System.Text.RegularExpressions.Regex.IsMatch(StringValue, @"\A\b[0-9a-fA-F]+\b\Z");
    }
  }

}
