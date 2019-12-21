using Telerik.WinControls.UI.Docking;

namespace ProjectStandard
{
  public static class XXToolWindow
  {
    private static void EventDockManagerDockStateChanging(object sender, DockStateChangingEventArgs e)
    {
      e.Cancel = (e.NewDockState == DockState.TabbedDocument) || (e.NewDockState == DockState.Floating);    
    }

    public static void ZZForbidMoving(this ToolWindow control)
    {
      control.DockManager.DockStateChanging += EventDockManagerDockStateChanging;
    }
  }
}

