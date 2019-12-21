using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectStandard
{
  public class TTReturnCodeFactory
  {

    public static readonly int ReturnCodeSuccess = 0;

    public static readonly int ReturnCodeError = -1;

    public static readonly int IdObjectError = -1;

    public const string Empty = "";

    public static TTReturnCode ErrorCode(string returnMessage = "", string returnNote = "")
    {
      return Create(ReturnCodeError, IdObjectError, returnMessage, returnNote);
    }

    public static TTReturnCode Create(int returnCode, int idObject, string returnMessage, string returnNote)
    {
      return new TTReturnCode(returnCode, idObject, returnMessage, returnNote);
    }

    public static TTReturnCode SuccessCode(string returnMessage = Empty, string returnNote = Empty, int idObject = 0)
    {
      return new TTReturnCode(ReturnCodeSuccess, idObject, returnMessage, returnNote);
    }
    
    public static async Task<TTReturnCode> FromHttpResponse(HttpResponseMessage response, bool KeepReturnCodeGivenByStoredProcedure)
    {
      TTReturnCode code;
      if (KeepReturnCodeGivenByStoredProcedure)
      {/* В данном варианте метод вернёт именно то значение TTReturnCode которое вернула ХП сервера */
        string json = await response.Content.ReadAsStringAsync();   code = TTConvert.JsonToObject<TTReturnCode>(json);
      }
      else
      { /* В данном варианте метод подменяет собой содержимое TTReturnCode которое вернула ХП сервера, возвращая своё собственное значение TTReturnCode на основе HttpResponse Code */
        code = response.IsSuccessStatusCode ? SuccessCode() : ErrorCode(response.ReasonPhrase, response.StatusCode.ToString());
      }
      return code;
    }

    public static async Task<TTReturnCode> FromHttpResponse(HttpResponseMessage response)
    {/* В данном варианте метод вернёт именно то значение TTReturnCode которое вернула ХП сервера */   

      if (response.StatusCode == TTServerCode.CodeErrorTimeout)
        return ErrorCode("Ошибка! Сервер не ответил на ваш запрос.");

      if (response.StatusCode == TTServerCode.CodeError)
        return ErrorCode("Произошла ошибка при выполнении запроса!");

      TTReturnCode code;
      try
      {
        string json = await response.Content.ReadAsStringAsync();
        code = TTConvert.JsonToObject<TTReturnCode>(json);
      }
      catch (Exception ex)
      {
        code = ErrorCode("Ошибка при попытке получения ответа от сервера! " + ex.Message + " " + ex.StackTrace, ex.Source);
      }

      return code;
    } 
  }
}
