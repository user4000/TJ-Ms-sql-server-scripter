using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using TJFramework;
using ProjectStandard;
using static MainProject.Program;
using static TJFramework.TJFrameworkManager;
using static ProjectStandard.TTListReturnCode;

namespace MainProject
{
  internal class TTSession 
  {
    internal string Id { get; private set; } = string.Empty;

    internal void GetSessionCode()
    {
      TTReturnCode code = TTReturnCodeFactory.SuccessCode("SessionID=1"); //Manager.Procedure.GetSessionCode();

      if (code.Success() == false)
        Ms.Message(MsgType.Error, "Идентификатор сессии не был получен", "Ошибка при попытке получения идентификатора сессии.", null, MsgPos.Unknown, 0).Create();
      else
        Id = code.ReturnMessage;
    }

    internal bool IsEmpty { get => Id == string.Empty; }
  }
}

