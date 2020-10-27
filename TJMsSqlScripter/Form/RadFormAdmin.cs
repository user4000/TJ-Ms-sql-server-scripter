using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using Telerik.WinControls;
using TJFramework;
using ProjectStandard;
using TJFramework.Extensions;
using static MainProject.Program;
using static TJFramework.TJFrameworkManager;
using static TJFramework.Logger.Manager;

namespace MainProject
{
  public partial class RadFormAdmin : RadForm, TJFramework.IEventStartWork, TJFramework.IEventEndWork, IWorkToExecute, IOutputMessage
  {
    internal IContainer MainContainer { get; set; } = null;
    internal TJFramework.Form.FxMain MainForm { get; set; } = null;

    internal TTJobManager JM = new TTJobManager();

    GridViewTextBoxColumn GcStart { get; set; }
    GridViewTextBoxColumn GcEnd { get; set; }

    public RadFormAdmin()
    {
      InitializeComponent();
      MainContainer = new Container();
    }

    public void EventStartWork()
    {
      BtnAddTimer.Click += (s, e) => EventAddTimerInterval();
      BtnDeleteTimer.Click += (s, e) => EventDeleteTimerInterval();
      BtnStart.Click += (s, e) => StartMainJob();
      BtnStop.Click += (s, e) => StopMainJob();
      BtnFolder.Click += (s, e) => SelectMainFolder();
      GridTimer.SortChanged += EventGridSortChanged;

      PageViewMain.Padding = new Padding(-5, 2, -5, -5);

      BtnStop.Enabled = false;

      JM.ListDays = Program.AppSettings.ListDays;
      JM.ListPeriod = Program.AppSettings.ListPeriod;

      if (JM.ListDays.Count != 7) { JM.ListDays.Clear(); TTDayFactory.Fill(JM.ListDays); }

      TTDayFactory.Bind(UListWeek, JM.ListDays);
      TTPeriodService.Bind(GridTimer, JM.ListPeriod, true);

      TxtIntervalMinute.ZZSetNonNegativeIntegerNumberOnly();

      GridTimer.ZZSetGridMainProperties();

      GcStart = (GridViewTextBoxColumn)GridTimer.Columns["GcStart"];
      GcEnd = (GridViewTextBoxColumn)GridTimer.Columns["GcEnd"];

      GcStart.Expression = string.Format("{0} + ':' + {1}", "IIF(GcHour1<10, '0', '') + CSTR(GcHour1)", "IIF(GcMinute1<10, '0', '') + CSTR(GcMinute1)");
      GcEnd.Expression = string.Format("{0} + ':' + {1}", "IIF(GcHour2<10, '0', '') + CSTR(GcHour2)", "IIF(GcMinute2<10, '0', '') + CSTR(GcMinute2)");

      GridTimer.Font = new Font("Verdana", 10);

      folderBrowserDialog.Description = "Specify the folder in which the script will be written";
      folderBrowserDialog.ShowNewFolderButton = true;
      folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
      if (Program.AppSettings.MainFolder == string.Empty) Program.AppSettings.MainFolder = AppDomain.CurrentDomain.BaseDirectory;
      TxtFolder.Text = Program.AppSettings.MainFolder;

      MainForm = TJFramework.TJFrameworkManager.MainForm;
      MainForm.Icon = MyNotifyIcon.Icon;
      MainForm.MyNotifyIcon.Icon = MyNotifyIcon.Icon;

      //MainForm.Resize += MainFormResize;
      //MyNotifyIcon.MouseDoubleClick += NotifyIconMouseDoubleClick;
    }


    /*
    private void MainFormResize(object sender, EventArgs e)
    {
      if (Program.AppSettings.MinimizeToSysTray)
        if (MainForm.WindowState == FormWindowState.Minimized) { MainForm.Hide(); MyNotifyIcon.Visible = true; }
    }

    private void NotifyIconMouseDoubleClick(object sender, MouseEventArgs e)
    {
      MainForm.Show(); MainForm.WindowState = FormWindowState.Normal; MyNotifyIcon.Visible = false;
    }
    */


    private void EventGridSortChanged(object sender, GridViewCollectionChangedEventArgs e)
    {
      GridTimer.SortChanged -= EventGridSortChanged;
      GridTimer.SortDescriptors.Clear();
      GridTimer.SortDescriptors.Add("GcNumber", ListSortDirection.Ascending);
      GridTimer.SortChanged += EventGridSortChanged;
    }

    private void EventDeleteTimerInterval()
    {
      if (GridTimer.Rows.Count < 1) return;
      try
      {
        GridViewRowInfo selectedRow = GridTimer.CurrentCell.RowInfo;
        GridTimer.Rows.Remove(selectedRow);
        GridTimer.BeginUpdate();
        TTPeriodService.SortByStartTime(JM.ListPeriod);
        GridTimer.EndUpdate();
        //Ms.Message(MsgType.Debug, "Строка удалена", "", BtnDeleteTimer, MsgPos.Unknown, 3);
        Ms.ShortMessage(MsgType.Debug, "Строка удалена", 200, BtnDeleteTimer, MsgPos.Unknown, 3).Create();
      }
      catch (Exception ex)
      {
        //Ms.Message(MsgType.Error, "Ошибка!", ex.Message, BtnDeleteTimer, MsgPos.Unknown, 3);
        Ms.Error("Ошибка!", ex).Control(BtnDeleteTimer).Delay(3).Create();
      }
    }

    private void EventAddTimerInterval()
    {
      DateTime x = new DateTime(2001, 1, 1);
      DateTime dt1 = TimePicker1.Value ?? x;
      DateTime dt2 = TimePicker2.Value ?? x;

      if ((dt1 == x) || (dt2 == x))
      {
        Ms.Message(MsgType.Warning, "Расписание не было добавлено", "Не выбран период времени", BtnAddTimer).Create() ;
        return;
      }

      if (dt1 >= dt2)
      {
        Ms.Message(MsgType.Warning, "Расписание не было добавлено", "Время окончания должно быть более поздним, чем время начала", BtnAddTimer).Create();
        return;
      }

      int interval = TTConvert.ToInt32(TxtIntervalMinute.Text, 0);

      if (interval < 1)
      {
        Ms.Message(MsgType.Warning, "Расписание не было добавлено", "Некорректное значение частоты запуска", BtnAddTimer).Create();
        return;
      }

      if ((dt2 - dt1).TotalMinutes <= 1)
      {
        Ms.Message(MsgType.Warning, "Расписание не было добавлено", "Слишком короткий отрезок времени", BtnAddTimer).Create();
        return;
      }

      if ((dt2 - dt1).TotalMinutes <= interval)
      {
        Ms.Message(MsgType.Warning, "Расписание не было добавлено", "Частота не соответствует указанному периоду времени", BtnAddTimer).Create();
        return;
      }

      TTPeriod period = TTPeriodService.Create(dt1, dt2, interval);

      GridTimer.BeginUpdate();
      bool FlagPeriodAdded = TTPeriodService.AddToList(JM.ListPeriod, period);
      GridTimer.EndUpdate();

      if (FlagPeriodAdded)
      {
        Ms.Message(MsgType.Ok, "Расписание добавлено", "", null, MsgPos.BottomLeft).Create();
      }
      else
      {
        Ms.Message(MsgType.Warning, "Расписание не было добавлено", "Интервал пересекается с уже существующими", BtnAddTimer, MsgPos.Unknown, 2).Create();
      }

    }

    private void ScriptingProgressEventHandler(object sender, ProgressReportEventArgs e)
    {
      if (e.Current.XPathExpression.Length > 2)
      {
        this.Invoke(new MethodInvoker(delegate
        {
          //TxtProgress.Text = e.Current.XPathExpression[2].GetAttributeFromFilter("Schema") + "." + e.Current.XPathExpression[2].GetAttributeFromFilter("Name");
          Print("-------------------------------------------------------------");
          //Print(TxtProgress.Text);
        }));
      }
      Application.DoEvents();
    }

    internal void SelectMainFolder()
    {
      DialogResult result = folderBrowserDialog.ShowDialog();
      if (result == DialogResult.OK)
      {
        Program.AppSettings.MainFolder = folderBrowserDialog.SelectedPath;
        TxtFolder.Text = Program.AppSettings.MainFolder;
      }
    }


    internal void EnableControlPanel(bool Enable)
    {
      PnGridTool.Enabled = Enable;
      PnInterval.Enabled = Enable;
      PnWeek.Enabled = Enabled;
      UListWeek.Enabled = Enable;
      BtnStart.Enabled = Enable;
      BtnDeleteTimer.Enabled = Enable;
      BtnStop.Enabled = !Enable;
    }

    public void Print(string message)
    {
      if (message.ToLower() == "{clear}") { TxtMessage.Clear(); return; }
      TxtMessage.Invoke(new MethodInvoker(delegate
      {
        TxtMessage.AppendText(TTConvert.Time + ": " + message + Environment.NewLine + Environment.NewLine);
      }));
    }

    internal void EventConnectedToDb()
    {
      JM.InitVariables(this);
      JM.InitConnection(Manager.Connection.GetNew());
    }

    internal void EventDisconnectedFromDb()
    {
      JM.StopMainJob();
      JM.InitConnection(null);
    }

    public void Execute() => JM.ScriptMaker.ScriptDatabaseObjects();

    internal void StartMainJob()
    {
      if (MainContainer == null)
      {
        Ms.Message(MsgType.Error, "", "Main container of the RadAdminForm is null", BtnStart).Create();
        return;
      }

      JM.StartMainJob();
    }

    internal void StopMainJob()
    {
      JM.StopMainJob();
    }

    public void EventEndWork()
    {
      StopMainJob();
      Program.AppSettings.ListPeriod = JM.ListPeriod;
      Program.AppSettings.ListDays = JM.ListDays;
    }

    public void OutputMessage(string message, string header = "")
    {
      Print((header == string.Empty) ? message : header + " " + message);
    }
  }
}
