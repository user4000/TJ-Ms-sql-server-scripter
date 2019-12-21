using System.Text;

namespace ProjectStandard
{

  public static class XXEncoding
  {
    public static Encoding GetEncoding(string encoding)
    {
      Encoding enc = null;
      try
      {
        enc = Encoding.GetEncoding(encoding);
      }
      catch
      {
        enc = Encoding.GetEncoding("CP866");
      }
      return enc;
    }
  }
}
