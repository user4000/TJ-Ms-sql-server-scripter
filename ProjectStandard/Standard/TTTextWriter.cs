using System.IO;
using System.Text;

namespace ProjectStandard
{
  public class TTTextWriter : TextWriter
  {

    private readonly IOutputMessage outputMessageDevice;

    public TTTextWriter(IOutputMessage OutputMessageDevice)
    {
      outputMessageDevice = OutputMessageDevice;
    }

    public override Encoding Encoding => Encoding.UTF8;

    public void OutputMessage(string message, string header="")
    {
      outputMessageDevice.OutputMessage(message);
      //base.WriteLine(message); // <--- No, no, no need to do that !
    }

    public override void WriteLine(string value) => OutputMessage(value);

    public override void Write(string value) => OutputMessage(value);

  }


}