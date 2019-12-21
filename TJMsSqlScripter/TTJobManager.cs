using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;
using TJFramework;
using ProjectStandard;
using static MainProject.Program;
using static TJFramework.TJFrameworkManager;
using static TJFramework.Logger.Manager;
using Microsoft.SqlServer.Management.Smo;

namespace MainProject
{
  internal class TTJobManager
  {
    private int IntervalSeconds { get; } = 29;

    private bool TimeToWork { get; set; }

    private DateTime TimeNow { get; set; }

    internal RadFormAdmin FormAdmin { get; private set; } = null;

    private IWorkToExecute Work { get; set; } = null;

    private SqlConnection Connection { get; set; } = null;

    internal TTScriptMaker ScriptMaker { get; set; } = new TTScriptMaker();

    internal TTJobFactory JobService { get; set; } = new TTJobFactory();

    internal BindingList<TTDay> ListDays { get; set; } = new BindingList<TTDay>();

    internal BindingList<TTPeriod> ListPeriod { get; set; } = new BindingList<TTPeriod>();

    internal List<TTJob> ListJob { get; set; } = new List<TTJob>();


    internal Timer MainTimer { get; set; }

    internal void InitVariables(RadFormAdmin form)
    {
      if (FormAdmin != null) return;
      FormAdmin = form;
      Work = form;
      ScriptMaker.InitOutputDevice(form);
      MainTimer = new Timer(form.MainContainer);
      MainTimer.Enabled = false;
      MainTimer.Interval = IntervalSeconds * 1000;
      MainTimer.Tick += EventMainTimer;
    }

    internal void InitConnection(SqlConnection connection)
    {
      Connection = connection;
      ScriptMaker.InitConnection(Connection);
      if (connection == null) return;

      ScriptMaker.InitScriptingOptions();

      bool SysAdmin = ScriptMaker.UserIsMemberOfSysAdminServerRole();
      bool DbOwnerOrDdlAdmin = false; string Error = string.Empty;

      if (SysAdmin==false)
      {

        DbOwnerOrDdlAdmin = ScriptMaker.UserHasEnoughPrivilegesInDatabase();

        if (DbOwnerOrDdlAdmin)
        {
          Error = "Вы не являетесь членом роли [sysadmin] сервера БД.";
          Ms.Message(MsgType.Debug, "Замечание", Error, null, MsgPos.Unknown, 0).Create();
          if (FormAdmin != null) FormAdmin.OutputMessage(Error);
        }
        else
        {
          Error = "Вы не являетесь членом роли [sysadmin] сервера БД, а также не являетесь членом ни одной из следующих встроенных ролей базы данных: [db_owner], [db_ddladmin], [db_securityadmin]. Это может повлиять на работу программы. Вы не сможете создать скрипт тех объектов, к которым у вас нет доступа. Программа предполагает работу от имени пользователя, имеющего достаточные привилегии в системе, которых у вас нет.";
          Ms.Message(MsgType.Fail, "Предупреждение!", Error, null, MsgPos.ScreenCenter, 3).Create();
          if (FormAdmin != null) FormAdmin.OutputMessage("Внимание! Возможна некорректная работа программы. " + Error);
        }
      }
    }






    internal bool CheckedDayExists()
    {
      foreach (TTDay item in ListDays)
        if (item.Checked) return true;
      return false;
    }

    internal bool CheckDay(DateTime date)
    {
      bool Flag = false;
      foreach (TTDay day in ListDays)
        if ((day.Checked) && ((day.Number == (int)date.DayOfWeek) || (day.Name == date.DayOfWeek.ToString()))) Flag = true;
      return Flag;
    }

    internal void ClearListJob()
    {
      foreach (TTJob job in ListJob)
      {
        JobService.Dispose(job);
      }
      ListJob.Clear();
    }

    internal void StartMainJob()
    {
      ClearListJob();

      foreach (TTPeriod period in ListPeriod)
      {
        TTJob job = JobService.Create(FormAdmin.MainContainer, period, Work);
        ListJob.Add(job);
      }

      if (CheckedDayExists() == false)
      {
        Ms.Message(MsgType.Error, "Запуск отменён", "Не был выбран ни один день недели.", FormAdmin.BtnStart, MsgPos.Unknown, 5).Create();
        return;
      }


      if (ListJob.Count < 1)
      {
        Ms.Message(MsgType.Error, "Запуск отменён", "Список расписаний пуст.", FormAdmin.BtnStart, MsgPos.Unknown, 5).Create();
        return;
      }

      FormAdmin.EnableControlPanel(false);
      FormAdmin.Print("{clear}");
      FormAdmin.Print("Запущен таймер для генерации скрипта объектов базы данных по расписанию.");
      Ms.Message("Start", "Запущен таймер для генерации скрипта объектов базы данных по расписанию.").NoAlert().Debug();
      //Ms.Message(MsgType.Info, "Запуск", "Запущен таймер для генерации скрипта объектов базы данных по расписанию.", FormAdmin.BtnStart, MsgPos.Unknown, 3);
      // **************************** //
      MainTimer.Enabled = true;
      // **************************** //
    }

    internal void StopMainJob()
    {
      bool TimerHasChangedItsState = false;
      if (MainTimer != null)
      {
        TimerHasChangedItsState = MainTimer.Enabled;
        MainTimer.Enabled = false;
      }
      ClearListJob();
      if (FormAdmin != null)
      {
        FormAdmin.EnableControlPanel(true);
        if (TimerHasChangedItsState) FormAdmin.Print("Остановлен таймер для генерации скрипта объектов базы данных по расписанию.");
      }
      if (TimerHasChangedItsState) Ms.Message("Stop", "Остановлен таймер для генерации скрипта объектов базы данных по расписанию.").NoAlert().Debug();
    }

    private void EventMainTimer(object sender, EventArgs e)
    {
      if (ListJob.Count < 1)
      {
        Ms.Message(MsgType.Warning, "Ошибка!", "Список расписаний пуст.", null, MsgPos.ScreenCenter, 7).Create();
        StopMainJob();
        return;
      }

      TimeNow = DateTime.Now; if (CheckDay(TimeNow) == false) return;

      foreach (TTJob job in ListJob)
      {
        TimeToWork = job.IsTimeToWork(TimeNow);
        if (job.JobTimer.Enabled != TimeToWork) // В этой ветке самое важно то, что таймер СМЕНИЛ состояние ON / OFF
        {
          job.JobTimer.Enabled = TimeToWork; // Если таймер только что включился он не выполнит тут же работу
          if (job.JobTimer.Enabled) job.ExecuteWork(); // Поэтому первый раз сделаем работу без таймера
        }
      }
    }
  }
}
