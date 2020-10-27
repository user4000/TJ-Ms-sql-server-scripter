using System;
using System.Linq;
using System.Text;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SecurityDriven.Inferno;
using SecurityDriven.Inferno.Kdf;
using SecurityDriven.Inferno.Mac;
using ProjectStandard.Extensions;
using System.Security.Authentication;

namespace ProjectStandard
{
  public class TTSecurityStandard
  {
    public static int Iterations { get; } = 16000;

    public static int ResultLengthBytes { get; } = 256;

    public static int ApiKeyIterations { get; } = 1024;

    public static int ApiKeyResultLengthBytes { get; } = 32;

    public static int LoginMinimumLength { get; } = 3;

    public static int SaltMinimumLength { get; } = 8;

    public static int SaltLength { get; } = 32;

    public static string Delimiter { get; } = "-";

    public static string Empty { get; } = string.Empty;

    private static PBKDF2 Pbkdf2 { get; set; }

    public static CryptoRandom CryptoRandomLocal { get; set; } = new CryptoRandom();

    public const string CharsForApiKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public const string CharsForLogin = "0123456789_ABCDEFGHIJKLMNOPQRSTUVWXYZqwertyuiopasdfghjklzxcvbnm";

    public const string CharsForSalt = "0123456789_!@#$%^&()+-=[]<>{}|;:,.?~ABCDEFGHIJKLMNOPQRSTUVWXYZqwertyuiopasdfghjklzxcvbnm";

    public const string CharsForPassword = CharsForSalt;

    public static int PasswordMinimumLength { get; } = 5;


    public static string HeaderHttpAuthorization { get; } = "Authorization";

    public static string HttpAuthTypeBasic { get; } = "Basic";

    public static string HttpAuthTypeApiKey { get; } = "APIKEY";

    public static string SeparatorLoginPassword { get; } = ":";

    public static string SeparatorSpace { get; } = " ";

    public static bool IsLetter(char c) => (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');

    public static bool IsDigit(char c) => (c >= '0') && (c <= '9');

    public static bool IsSymbol(char c) => (c > 32) && (c < 127) && !IsDigit(c) && !IsLetter(c);

    public static bool IsPasswordLongEnough(string password) => password.Length >= PasswordMinimumLength;

    public static bool IsValidValue(string value, string CharSet)
    {
      bool result = true;
      foreach (char c in value)
      {
        if (CharSet.Contains(c) == false) { result = false; break; }
      }
      return result;
    }

    public static bool IsValidPassword(string password) => IsValidValue(password, CharsForPassword);

    public static bool IsValidLogin(string login) => IsValidValue(login, CharsForLogin);

    public static int GetRandom(int A, int B) => CryptoRandomLocal.Next(A, B);

    public static byte[] GetBytes(string s) => Utils.SafeUTF8.GetBytes(s);

    public static string GetString(byte[] array) => Utils.SafeUTF8.GetString(array);

    public static string GenerateSalt()
    {
      return new string(Enumerable.Repeat(CharsForSalt, SaltLength).Select(s => s[CryptoRandomLocal.Next(s.Length)]).ToArray());
    }

    public static string GenerateRandomString(int Length)
    {
      return new string(Enumerable.Repeat(CharsForSalt, Length).Select(s => s[CryptoRandomLocal.Next(s.Length)]).ToArray());
    }

    public static string ComputeHash(string Password, string Salt)
    {
      if (Salt.Length < SaltMinimumLength) { return string.Empty; }

      Pbkdf2 = new PBKDF2 // TODO: Если подать на вход строку Salt меньше 8 байтов - то этот метод-конструктор выдает исключение ! //
       (
         HMACFactories.HMACSHA512,
         password: Encoding.ASCII.GetBytes(Password),
         salt: Encoding.ASCII.GetBytes(Salt),
         iterations: Iterations
       );

      byte[] HashSalted = Pbkdf2.GetBytes(ResultLengthBytes);

      Pbkdf2.Dispose(); Pbkdf2 = null;

      return BitConverter.ToString(HashSalted).Replace(Delimiter, Empty);
    }

    public static AuthenticationHeaderValue CreateBasicAuthenticationHeader(string user, string password)
    {
      return
        new AuthenticationHeaderValue(
            HttpAuthTypeBasic,
            Convert.ToBase64String(
                Encoding.ASCII.GetBytes(user + SeparatorLoginPassword + password)));
    }

    public static TTReturnCode DecodeBasicAuthenticationHeader(string HttpHeader)
    {
      string base64 = HttpHeader.Right(HttpHeader.Length - HttpAuthTypeBasic.Length).Trim();
      byte[] bytes = Convert.FromBase64String(base64);
      string LoginAndPassword = Encoding.ASCII.GetString(bytes);
      int index = LoginAndPassword.IndexOf(SeparatorLoginPassword);
      string Login = LoginAndPassword.Left(index);
      string Password = LoginAndPassword.Right(LoginAndPassword.Length - index - 1);
      TTReturnCode code = new TTReturnCode(0, 0, Login, Password);
      return code;
    }

    public static AuthenticationHeaderValue CreateApikeyAuthenticationHeader(string apikey) => new AuthenticationHeaderValue(HttpAuthTypeApiKey, apikey);

    public static string DecodeApikeyAuthenticationHeader(string HttpHeader)
    {
      return HttpHeader.Right(HttpHeader.Length - HttpAuthTypeApiKey.Length).Trim();
    }

    public static string GenerateApiKey()
    {
      byte[] array = CryptoRandomLocal.NextBytes(1024);
      string Salt = GenerateSalt();
      if (Salt.Length < 8) { throw new AuthenticationException("Ошибка! Не удалось сгенерировать API key. Длина соли меньше 8 байтов."); }

      Pbkdf2 = new PBKDF2 // TODO: Если подать на вход строку Salt меньше 8 байтов - то этот метод-конструктор выдает исключение ! //
       (
         HMACFactories.HMACSHA512,
         password: array,
         salt: Encoding.ASCII.GetBytes(Salt),
         iterations: ApiKeyIterations
       );

      byte[] HashSalted = Pbkdf2.GetBytes(ApiKeyResultLengthBytes);

      Pbkdf2.Dispose(); Pbkdf2 = null;

      string Result = string.Empty; int j = 0; int k = CharsForApiKey.Length;

      for (int i = 0; i < HashSalted.Length; i++)
      {
        j = HashSalted[i] % k;
        Result += CharsForApiKey[j];
      }

      return Result;
    }

    public static byte[] Encrypt(byte[] masterKey, ArraySegment<byte> plaintext, ArraySegment<byte>? salt = null)
    { // Does not work in Windows 7 //
      return SuiteB.Encrypt(masterKey, plaintext, salt);
    }

    public static byte[] Decrypt(byte[] masterKey, ArraySegment<byte> ciphertext, ArraySegment<byte>? salt = null)
    { // Does not work in Windows 7 //
      return SuiteB.Decrypt(masterKey, ciphertext, salt);
    }

  }
}
