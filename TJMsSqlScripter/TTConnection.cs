using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using TJFramework;
using static TJFramework.TJFrameworkManager;
using static MainProject.Program;
using ProjectStandard;

namespace MainProject
{
  internal class TTConnection : ILoginFormManager, ISqlConnectionFactory
  {
    private string TxtNotConnected { get; } = "Не удалось подключиться к базе данных.";
    private string TxtConnected { get; } = "Подключение к базе данных прошло успешно.";

    internal TTSession Session { get; private set; } = new TTSession();

    private bool ConnectionIsEstablished { get; set; } = false;
    public bool IsConnected { get => ConnectionIsEstablished; }

    internal string ConnectionStringTemplate { get => AppSettings.DBConnectionTemplate; }
    internal string Server { get => AppSettings.DBServerName; }
    internal string Database { get => AppSettings.DBName; }

    private string LastSuccessConnectionString { get; set; } = string.Empty;

    private string BuildConnectionString(string Login, string Password)
    {
      return ConnectionStringTemplate
              .Replace("<SERVER>", Server)
              .Replace("<DATABASE>", Database)
              .Replace("<LOGIN>", Login)
              .Replace("<PASSWORD>", Password);
    }

    public async Task<bool> Disconnect()
    {
      ConnectionIsEstablished = false;
      Manager.EventDisconnectedFromDB();
      await Task.Delay(10);
      return true;
    }

    private void ShowMessageSuccess(System.Windows.Forms.Control control)
    {
      if (control == null)
        Ms.Message(MsgType.Ok, "Подключение установлено", TxtConnected, null, MsgPos.ScreenCenter, 4).Create();
      else
        Ms.Message(MsgType.Ok, "Подключение установлено", TxtConnected, control, MsgPos.Unknown, 4).Create();
    }

    private void ShowMessageFailure(Exception ex, System.Windows.Forms.Control control)
    {
      if (control == null)
        Ms.Message(MsgType.Warning, "Ошибка!", TxtNotConnected, null, MsgPos.ScreenCenter, 6).Create();
      else
        Ms.Message(MsgType.Warning, "Ошибка!", TxtNotConnected, control, MsgPos.Unknown, 6).Create();
      Ms.Message(MsgType.Error, ex.Source, ex.Message, null, MsgPos.Unknown, 0).Create();
    }

    public SqlConnection GetNew()
    {
      return new SqlConnection(LastSuccessConnectionString);
    }

    public async Task<bool> Connect(string Login, string Password, System.Windows.Forms.Control control = null)
    {
      ConnectionIsEstablished = false;
      string s = BuildConnectionString(Login, Password);

      SqlConnection con = new SqlConnection(s);

      try { await con.OpenAsync(); }
      catch (Exception ex) { ShowMessageFailure(ex, control); }

      if (con.State == ConnectionState.Open)
      {
        AppSettings.DBLastLogin = Login;
        AppSettings.HiddenDBLastSuccessLogin = DateTime.Now;
        LastSuccessConnectionString = s;
        ConnectionIsEstablished = true;
        con.Close();
      }

      /*if (ConnectionIsEstablished)
      {
        Session.GetSessionCode();
        if (Session.IsEmpty)
        {
          ConnectionIsEstablished = false;
          Ms.Message(MsgType.Warning, "Произошла ошибка!", "Не удалось получить идентификатор сессии", null, MsgPos.ScreenCenter, 4);
        }
      }*/

      if (ConnectionIsEstablished)
      {
        ShowMessageSuccess(control);
        Manager.EventConnectedToDB();
        //Manager.Debug(Session.Id, "Session Id");
        if (AppSettings.AutoConnectToDb) AppSettings.LastIdSession = TTRat.TransformF(Password);
      }

      return ConnectionIsEstablished;
    }

    public Task<TTReturnCode> ChangePassword(string Login, string OldPassword, string NewPassword)
    {
      TTReturnCode code = TTReturnCodeFactory.ErrorCode("This feature does not work.");
      Task<TTReturnCode> task = new Task<TTReturnCode>(() => code);
      return task;
    } 

    public string LastLogin() => AppSettings.DBLastLogin;

  }
}
