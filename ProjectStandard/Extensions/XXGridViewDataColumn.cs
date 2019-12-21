using System;
using Telerik.WinControls.UI;

namespace ProjectStandard
{
  public static class XXGridViewDataColumn
  {
    public static string ZZGuid(this GridViewDataColumn column) => column.Name + "-" + Guid.NewGuid().ToString();

  }
}

