using System;
using System.IO;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace ProjectStandard
{
  public class TTConvert
  {
    public static string DateTimeFormat { get; } = "yyyy-MM-dd HH:mm:ss";

    public static string DateTimeFormatAsFileName { get; } = "yyyy-MM-dd_HH-mm-ss";

    public static string HttpContentJson { get; } = "application/json";

    public static string Time { get => DateTime.Now.ToString(DateTimeFormat); }


    public static string TimeAsFileName { get => DateTime.Now.ToString(DateTimeFormatAsFileName); }


    public static string ToString(DateTime dt) => dt.ToString(DateTimeFormat);


    public static int ToInt32(string value, int Default)
    {
      int f = Default;
      if (Int32.TryParse(value, out int x)) f = x;
      return f;
    }

    public static int ToInt32(object value, int Default)
    {
      int f = Default;
      if (Int32.TryParse(value.ToString(), out int x)) f = x;
      return f;
    }

    public static string ObjectToJson(object value) => JsonConvert.SerializeObject(value);

    public static StringContent ObjectToJsonStringContent(object value) => new StringContent(ObjectToJson(value), Encoding.UTF8, HttpContentJson);

    public static StringContent CreateStringContent(string json) => new StringContent(json, Encoding.UTF8, HttpContentJson);


    public static T JsonToObject<T>(string json) => JsonConvert.DeserializeObject<T>(json);

    public static T ReturnCodeToObject<T>(TTReturnCode code) => JsonToObject<T>(code.ReturnMessage);

    public static T ByteArrayFirstToJsonThenToObject<T>(byte[] array) => JsonToObject<T>(ByteArrayToString(array));

    public static byte[] ObjectToByteArray(object MyObject)
    {
      if (MyObject == null)
        return null;
      BinaryFormatter bf = new BinaryFormatter();
      using (MemoryStream ms = new MemoryStream())
      {
        bf.Serialize(ms, MyObject);
        return ms.ToArray();
      }
    }

    public static Object ByteArrayToObject(byte[] array)
    {
      MemoryStream memStream = new MemoryStream();
      BinaryFormatter binForm = new BinaryFormatter();
      memStream.Write(array, 0, array.Length);
      memStream.Seek(0, SeekOrigin.Begin);
      Object obj = (Object)binForm.Deserialize(memStream);

      return obj;
    }

    public static string ByteArrayToString(byte[] array) => System.Text.Encoding.UTF8.GetString(array, 0, array.Length);

    public static ByteArrayContent ObjectToByteArrayContent(object MyObject)
    {
      if (MyObject == null) return null;
      byte[] data = ObjectToByteArray(MyObject);
      ByteArrayContent byteContent = new ByteArrayContent(data);
      return byteContent;
    }

    public static async Task<string> ReadContentAsString(HttpResponseMessage message)
    {
      Stream receiveStream = await message.Content.ReadAsStreamAsync();
      StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
      return readStream.ReadToEnd();
    }

  }
}


