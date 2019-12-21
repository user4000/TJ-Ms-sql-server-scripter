using System;

namespace ProjectStandard
{
  [Serializable]
  public class TTReturnCode
  {
    public int ReturnCode { get; set; } = -1;

    public int IdObject { get; set; } = -1;

    public string ReturnMessage { get; set; } = string.Empty;

    public string ReturnNote { get; set; } = string.Empty;

    public TTReturnCode() { /* CONSTRUCTOR */ }

    /* CONSTRUCTOR */
    public TTReturnCode(int returnCode, int idObject, string returnMessage, string returnNote)
    {
      ReturnCode = returnCode; IdObject = idObject; ReturnMessage = returnMessage; ReturnNote = returnNote;
    }

    public bool Success() => ReturnCode == 0;

    public bool Error() => !Success();

  }
}
