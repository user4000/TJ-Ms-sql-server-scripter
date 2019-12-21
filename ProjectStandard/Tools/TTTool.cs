using System.IO;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ProjectStandard
{
  public class TTTool
  {
    public static string GetDateOfPdbFile() 
    {
      return TTConvert.ToString
        (
        File.GetLastWriteTime($"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.pdb")
        );
    }
  }
}
