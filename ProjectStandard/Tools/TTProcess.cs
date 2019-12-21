using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectStandard.Tools
{
  public class TTProcess
  {
    public static string Execute(string command, string parameter, string encoding = "CP866")
    {
      string parameters = parameter;
      string output = string.Empty;
      string error = string.Empty;

      Encoding enc = XXEncoding.GetEncoding(encoding);

      ProcessStartInfo psi = new ProcessStartInfo(command, parameters);

      psi.RedirectStandardOutput = true;
      psi.RedirectStandardError = true;    
      psi.UseShellExecute = false;
      psi.CreateNoWindow = true;
      psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
      psi.StandardOutputEncoding = enc;

      System.Diagnostics.Process process = System.Diagnostics.Process.Start(psi);

      using (System.IO.StreamReader myOutput = process.StandardOutput)
      {
        output = myOutput.ReadToEnd();
      }

      using (System.IO.StreamReader myError = process.StandardError)
      {
        error = myError.ReadToEnd();
      }

      output = output + " " + error;

      return output;    
    }
  }
}
