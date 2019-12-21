using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ProjectStandard
{
  public partial class RadFormStart : Telerik.WinControls.UI.RadForm
  {
    public RadFormStart()
    {
      InitializeComponent();
    }

    private void radButton1_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
