namespace ProjectStandard
{
  public class TTReturnCodeFormatter
  {
    public static TTReturnCode TryToGetReturnCodeFromJson(string json)
    {
      TTReturnCode code = null;

      if (MayBeReturnCode(json))
        try
        {
          code = TTConvert.JsonToObject<TTReturnCode>(json);
        }
        catch { code = null;  }

      return code;
    }


    public static bool MayBeReturnCode(string json) =>
      json.Contains("\"" + nameof(TTReturnCode.ReturnCode)    + "\"") &&
      json.Contains("\"" + nameof(TTReturnCode.ReturnMessage) + "\"") &&
      json.Contains("\"" + nameof(TTReturnCode.IdObject)      + "\"") &&
      json.Contains("\"" + nameof(TTReturnCode.ReturnNote)    + "\"");
    

    public static string ToString(TTReturnCode code)
    {
      return $"{code.ReturnCode};{code.IdObject};{code.ReturnMessage};{code.ReturnNote}";
    }
  }
}
