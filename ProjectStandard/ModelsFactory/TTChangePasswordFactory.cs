namespace ProjectStandard
{
  using Model;
  public class TTChangePasswordFactory
  {
    public static TTChangePassword Create(string login, string oldPassword, string newPassword )
    {
      return new TTChangePassword() { Login = login, OldPassword = oldPassword, NewPassword = newPassword };
    }
  }
}
