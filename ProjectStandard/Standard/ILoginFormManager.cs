using System.Threading.Tasks;

namespace ProjectStandard
{
  public interface ILoginFormManager
  {
    bool IsConnected { get; }

    Task<bool> Connect(string Login, string Password, System.Windows.Forms.Control control = null);

    Task<bool> Disconnect();

    string LastLogin();

    Task<TTReturnCode> ChangePassword(string Login, string OldPassword, string NewPassword);

  }
}