using System.Collections.Generic;

namespace ProjectStandard
{
  public static class XXIListReturnCode
  {
    public static TTReturnCode ZZFirst<TTReturnCode>(this IList<TTReturnCode> items) where TTReturnCode : new()
    {
      TTReturnCode code = default(TTReturnCode);
      try { code = items[0]; } catch { }
      return code;
    }
  }
}

