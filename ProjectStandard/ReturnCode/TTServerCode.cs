using System.Net;
using System.Net.Http;

namespace ProjectStandard
{
  public class TTServerCode
  {
    //public static string ContentTextPlain { get; } = "text/plain";

    //public static string ContentJson { get; } = "application/json";

    public static int ErrorApikeyExpired { get; } = 111111;

    public static int ErrorApikeyWrong { get; } = 111112;

    public static int ErrorUserIsNotFound { get; } = 100005;

    public static int ErrorUserIsLocked { get; } = 100004;

    public static int ErrorIncorrectLoginOrPassword { get; } = 100003;

    public static int ErrorStoredProcedureExecution { get; } = 100007;


    public static HttpStatusCode CodeApikeyIsExpired { get; } = HttpStatusCode.Conflict;

    public static HttpStatusCode CodeApikeyIsWrong { get; } = HttpStatusCode.Unauthorized;

    public static HttpStatusCode CodeClientSentNoData { get; } = HttpStatusCode.BadRequest;

    public static HttpStatusCode CodeErrorTimeout { get; } = HttpStatusCode.ServiceUnavailable;

    public static HttpStatusCode CodeError { get; } = HttpStatusCode.Forbidden;






    public static bool IsApikeyExpired(TTReturnCode code) => code.ReturnCode == ErrorApikeyExpired;

    public static bool IsApikeyExpired(HttpResponseMessage response) => response.StatusCode == CodeApikeyIsExpired;

  }
}
