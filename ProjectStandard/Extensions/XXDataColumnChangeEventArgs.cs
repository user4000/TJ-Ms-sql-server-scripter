using System.Data;

namespace ProjectStandard
{
  public static class XXDataColumnChangeEventArgs
  {
    public static string ZzProposedValue(this DataColumnChangeEventArgs e)
    {
      return e.ProposedValue==null ? string.Empty : e.ProposedValue.ToString();
    }
  }
}
