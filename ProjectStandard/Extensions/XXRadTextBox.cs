using Telerik.WinControls.UI;

namespace ProjectStandard
{
  public static class XXRadTextBox
  {

    public static void ZzSetValue(this RadTextBox radTextBox, long value) => radTextBox.Text = value.ToString();

    public static void ZzSetValue(this RadTextBox radTextBox, int value) => radTextBox.Text = value.ToString();

  }
}

