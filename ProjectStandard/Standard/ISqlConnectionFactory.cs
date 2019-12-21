using System.Data.SqlClient;

namespace ProjectStandard
{
  public interface ISqlConnectionFactory
  {
    SqlConnection GetNew();
  }
}