using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Telerik.WinControls.UI;
using TJFramework;
using TJFramework.FrameworkSettings;
using ProjectStandard;

namespace MainProject
{
  internal static class Program
  {
    internal static TTManager Manager { get; } = TTManagerFactory.Create();
    internal static TTProjectSettings AppSettings { get => TJFrameworkManager.ApplicationSettings<TTProjectSettings>(); }
    internal static TJStandardFrameworkSettings FrameworkSettings { get => TJFrameworkManager.FrameworkSettings; }


    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      TJFrameworkManager.Logger.FileSizeLimitBytes = 1000000;
      TJFrameworkManager.Logger.Create("TJ_Ms_Sql_Server_Scripter");

      TJFrameworkManager.Service.SetMainFormCaption("Database object scripter for MS Sql Server");

      TJFrameworkManager.Service.CreateApplicationSettings<TTProjectSettings>();
      
      TJFrameworkManager.Service.AddForm<RadFormLogin>("Вход", true, true);
      TJFrameworkManager.Service.AddForm<RadFormAdmin>("Администратор", true, true);
      TJFrameworkManager.Service.StartPage<RadFormLogin>();

      FrameworkSettings.VisualEffectOnStart = true;
      FrameworkSettings.VisualEffectOnExit = true;
      FrameworkSettings.MainFormMinimizeToTray = true;

      FrameworkSettings.HeaderFormSettings = "Настройки";
      FrameworkSettings.HeaderFormLog = "Сообщения";
      FrameworkSettings.HeaderFormExit = "Выход";

      FrameworkSettings.ValueColumnWidthPercent = 40;
      FrameworkSettings.PageViewFont = new Font("Verdana", 9);
      FrameworkSettings.PageViewItemSize = new Size(150, 27);
      FrameworkSettings.ItemSizeMode = PageViewItemSizeMode.EqualHeight;
      FrameworkSettings.MainPageViewReducePadding = true;
      FrameworkSettings.TabMinimumWidth = 140;

      Action ExampleOfVisualSettings = () =>
      {
        FrameworkSettings.PageViewFont = new Font("Verdana", 12); // <-- customize font of Main Page View //
        FrameworkSettings.MainFormMargin = 50;
        FrameworkSettings.PageViewItemSize = new Size(200, 20);
        FrameworkSettings.ItemSizeMode = PageViewItemSizeMode.EqualSize;
        FrameworkSettings.PageViewItemSpacing = 50;
        FrameworkSettings.MaxAlertCount = 4;
        FrameworkSettings.SecondsAlertAutoClose = 3;
        FrameworkSettings.RememberMainFormLocation = true;
        //TJMainFormManager.Service.SetEventSelectedPageChanged(TestEventPageChanged);
      };

      //ExampleOfVisualSettings.Invoke();

      FrameworkSettings.RememberMainFormLocation = true;


      TJFrameworkManager.Service.EventAfterAllFormsAreCreated = async () => await Manager.EventAfterAllFormsAreCreated();
      TJFrameworkManager.Service.EventBeforeMainFormClose = () => Manager.EventEndWork();
      TJFrameworkManager.Service.EventPageChanged = Manager.EventSelectedPageChanged;
      TJFrameworkManager.Run();
    }
  }
}