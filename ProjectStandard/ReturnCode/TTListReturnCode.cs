using System.Collections.Generic;
using System.Linq;

namespace ProjectStandard
{
  public class TTListReturnCode
  {
    public static TTReturnCode First(IList<TTReturnCode> list)
    {
      TTReturnCode code = default(TTReturnCode);
      try
      {
        code = list.First();
      }
      catch
      {
        code = TTReturnCodeFactory.ErrorCode("Error! Procedure did not return any data.", "Ошибка! Процедура не вернула результатов.");
      }
      return code;
    }
  }
}
