using System;
using System.Threading.Tasks;
using ProjectStandard;
using TJFramework;
using static MainProject.Program;
using static TJFramework.TJFrameworkManager;
using static TJFramework.Logger.Manager;

namespace MainProject
{
  internal class TTManager
  {
    internal TTConnection Connection { get; set; } = null;

    internal RadFormLogin RefRadFormLogin { get => Pages.GetRadForm<RadFormLogin>(); }

    internal RadFormAdmin RefRadFormAdmin { get => Pages.GetRadForm<RadFormAdmin>(); }

    internal bool FlagExecuteFirstTime { get; private set; } = false;

    internal void Debug(string s, string h = "")
    {
      Ms.Message(MsgType.Debug, h, s, null, MsgPos.Unknown, 0).Create();
    }

    internal void BindLoginFormWithConnectionInstance()
    {
      RadFormLogin form = Pages.GetRadForm<RadFormLogin>();
      ILoginFormManager managerConnection = Connection;
      form?.InitLoginFormManager(managerConnection);
    }

    internal async Task EventAfterAllFormsAreCreated()
    {
      BindLoginFormWithConnectionInstance();

      RefRadFormLogin.PasswordText = "";

      if (AppSettings.AutoConnectToDb)
        try
        {
          Manager.RefRadFormLogin.PasswordText = TTRat.TransformB(AppSettings.LastIdSession);
        }
        catch
        {
          Manager.RefRadFormLogin.PasswordText = "";
        }

      await Task.Delay(1000);

      if (AppSettings.AutoConnectToDb) await Manager.RefRadFormLogin.ConnectButton();
    }

    private void EnableChildForms(bool Enable)
    {
      Pages.EnablePage<RadFormAdmin>(Enable);
      if (Enable) RefRadFormAdmin.EnableControlPanel(true);
    }

    internal void EventDisconnectedFromDB()
    {
      EnableChildForms(false);
      RefRadFormAdmin.EventDisconnectedFromDb();
    }

    internal void EventConnectedToDB()
    {
      EnableChildForms(true);
      Pages.GotoPage<RadFormAdmin>();
      RefRadFormAdmin.EventConnectedToDb();
    }

    internal void EventSelectedPageChanged(string PageUniqueName)
    {
      /*if (PageUniqueName == typeof(RadFormAdmin).FullName)
      {
        RefRadFormAdmin.EventUserSelectedThisForm();
      }*/
    }

    internal void EventEndWork()
    {    
      //if (USettings.OutputMessageToFile) Log.Save(MsgType.Debug,"","Application is closed.");
    }
  }
}
