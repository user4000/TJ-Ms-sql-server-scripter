using System.Net;
using System.Net.NetworkInformation;
using ProjectStandard.Tools;

namespace ProjectStandard
{
  public class TTNetwork
  {

    public static string NetshHttpShowSslCert(string CommandEncoding)
    {
      string Command = "netsh";
      string Argument = "http show sslcert"; // Запускаем эту команду и разбираем ответ, ищем в ответе хэш сертификата //
      string Result = TTProcess.Execute(Command, Argument, CommandEncoding);
      return Result;
    }

    public static bool IsTcpPortAvailable(int Port /*, IOutputMessage device*/ )
    {   
      bool isAvailable = true;
      // https://stackoverflow.com/questions/570098/in-c-how-to-check-if-a-tcp-port-is-availablehttps://stackoverflow.com/questions/570098/in-c-how-to-check-if-a-tcp-port-is-available
      // Evaluate current system tcp connections. This is the same information provided
      // by the netstat command line application, just in .Net strongly-typed object
      // form.  We will look through the list, and if our port we would like to use
      // in our TcpClient is occupied, we will set isAvailable to false.

      IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
      TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
      IPEndPoint[] ipEndPointArray = ipGlobalProperties.GetActiveTcpListeners();

      if (isAvailable)
      foreach (IPEndPoint item in ipEndPointArray)    
        if (item.Port == Port) { isAvailable = false; break;}

      if (isAvailable)
      foreach (TcpConnectionInformation tcpInfo in tcpConnInfoArray)    
        if (tcpInfo.LocalEndPoint.Port == Port) { isAvailable = false; break;}
      
      return isAvailable;
    }
  }
}
