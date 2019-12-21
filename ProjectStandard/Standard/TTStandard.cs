using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectStandard
{
  public static class TTStandard
  {

    public static char MessageHeaderSeparator { get; } = '|';

    public static string GridColumnPrefix { get; } = "Cc";

    public static string GetGridColumnName(string ColumnName) => $"{GridColumnPrefix}{ColumnName}";

    public static string HeaderAndMessage(string header, string message)
    {
      return header + MessageHeaderSeparator + message;
    }

    public static Tuple<string, string> HeaderAndMessage(string MessageWithHeader)
    {
      string header, message; header = message = string.Empty; int count = 0;
      string[] words = MessageWithHeader.Split(MessageHeaderSeparator);    
      foreach (string word in words) if (++count == 1) { header = word; } else { message += word; }
      return Tuple.Create(header, message);
    }

    public static int CheckRange(int MinValue, int MaxValue, int Variable)
    {
      if (MinValue > MaxValue) MinValue = MaxValue;
      if (Variable > MaxValue) Variable = MaxValue;
      if (Variable < MinValue) Variable = MinValue;
      return Variable;
    }
  }
}
