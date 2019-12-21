using System;
using System.Windows.Forms;
using Telerik.WinControls.UI.Docking;

namespace ProjectStandard
{
  public static class XXDocumentWindow
  {
    private static void EventDocumentWindowMove(object sender, EventArgs e)
    {
      if (sender is DocumentWindow == false) return;
      DocumentWindow window = (sender as DocumentWindow);
      if (window.DockState!=DockState.TabbedDocument) window.DockState = DockState.TabbedDocument;
    }

    public static void ZzDoNotMoveTheWindow(this DocumentWindow window)
    {
      window.Move += EventDocumentWindowMove;
    }
  }
}
