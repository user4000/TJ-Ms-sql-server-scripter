using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ProjectStandard
{
  public static class XXRadTextBoxControl
  {
    public static void EventKeyPressNonNegativeIntegerNumberOnly(object s, KeyPressEventArgs e)
    {
      if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) /* && (e.KeyChar != '.') */ ) e.Handled = true; 
    }

    public static void ZxSetNonNegativeIntegerNumberOnly(this RadTextBoxControl control)
    {
      control.KeyPress += (s, e) => EventKeyPressNonNegativeIntegerNumberOnly(s, e);
    }
  }
}

