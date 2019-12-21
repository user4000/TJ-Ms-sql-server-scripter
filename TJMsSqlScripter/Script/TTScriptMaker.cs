using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using TJFramework;
using ProjectStandard;
using static MainProject.Program;
using static TJFramework.TJFrameworkManager;
using static TJFramework.Logger.Manager;
using System.IO;

namespace MainProject
{
  internal class TTScriptMaker
  {
    private string IsSystemObject { get; } = "IsSystemObject";

    internal bool ScriptingIsInProgress { get; set; } = false;

    internal SqlConnection Connection { get; set; } = null;

    internal IOutputMessage OutputDevice { get; set; } = null;

    internal Dictionary<Urn, string> Objects { get; set; } = new Dictionary<Urn, string>();


    internal ScriptingOptions scriptingOptions { get; set; } = null;


    internal TTScriptMaker() { }

    internal void InitConnection(SqlConnection connection) => Connection = connection;

    internal void InitOutputDevice(IOutputMessage device) => OutputDevice = device;

    internal void InitScriptingOptions()
    {
      scriptingOptions = new ScriptingOptions();
      scriptingOptions.Permissions = true;
      scriptingOptions.ExtendedProperties = true;
      scriptingOptions.ScriptDrops = false;
      scriptingOptions.IncludeDatabaseContext = true;
      scriptingOptions.NoCollation = false;
      scriptingOptions.NoFileGroup = true;
      scriptingOptions.NoIdentities = false;
      scriptingOptions.AllowSystemObjects = false;
      scriptingOptions.ClusteredIndexes = true;
      scriptingOptions.ConvertUserDefinedDataTypesToBaseType = false;
      scriptingOptions.Default = true;
      scriptingOptions.Encoding = Encoding.UTF8;
      scriptingOptions.Permissions = true;
      scriptingOptions.ScriptBatchTerminator = true;
      scriptingOptions.ScriptSchema = true;
      scriptingOptions.ColumnStoreIndexes = true;
      scriptingOptions.DriAllConstraints = true;
      scriptingOptions.DriAll = true;
      scriptingOptions.TargetServerVersion = Program.AppSettings.MsSqlVersion;
    }


    internal bool UserIsMemberOfSysAdminServerRole()
    {
      bool result = false;
      try
      {
        ServerConnection serverConnection = new ServerConnection(Connection);
        Server server = new Server(serverConnection);
        Login login = server.Logins[server.ConnectionContext.Login];
        result = login.IsMember("sysadmin");
        login = null; server = null; serverConnection.Disconnect();
      }
      catch { result = false; }
      return result;
    }

    internal bool UserHasEnoughPrivilegesInDatabase()
    {
      bool result = false;
      try
      {
        ServerConnection serverConnection = new ServerConnection(Connection);
        Server server = new Server(serverConnection);
        Login login = server.Logins[server.ConnectionContext.Login];
        Database db = server.Databases[Program.AppSettings.DBName];

        foreach (User user in db.Users)
          if (user.Login == login.Name)
          {
            result =
              (
              (user.IsMember("db_owner")) ||
              (user.IsMember("db_ddladmin")) ||
              (user.IsMember("db_securityadmin"))
              ); break;
          }
        db = null; login = null; server = null; serverConnection.Disconnect();
      }
      catch { result = false; }
      return result;
    }


    internal string GetFullName(Urn urn)
    {
      if (urn.XPathExpression.Length > 2)
        return
          "[" + urn.XPathExpression[1].GetAttributeFromFilter("Name") + "]." +
          "[" + urn.XPathExpression[2].GetAttributeFromFilter("Schema") + "]." +
          "[" + urn.XPathExpression[2].GetAttributeFromFilter("Name") + "]";

      else return urn.Value;
    }


    private string ScriptObject(Urn[] urns, Scripter scripter)
    {
      StringCollection stringCollection = scripter.Script(urns);
      StringBuilder stringBuilder = new StringBuilder();

      Urn urn = urns[0];

      string ObjectFullName = urn.Value;
      string ObjectType = urn.Type;

      if (ScriptingIsInProgress == false)
      {
        stringBuilder.Append("/* It is a bug! This string never appears in this script text !!! */" + Environment.NewLine + Environment.NewLine);
        ScriptingIsInProgress = true;
      }

      if (urn.XPathExpression.Length > 2)
      {
        ObjectFullName =
          "[" + urn.XPathExpression[1].GetAttributeFromFilter("Name") + "]." +
          "[" + urn.XPathExpression[2].GetAttributeFromFilter("Schema") + "]." +
          "[" + urn.XPathExpression[2].GetAttributeFromFilter("Name") + "]";
      }

      stringBuilder.Append($"/* --- {ObjectType} --- {ObjectFullName} --- */" + Environment.NewLine + Environment.NewLine);

      foreach (string str in stringCollection)
      {
        stringBuilder.Append
          (
          str + Environment.NewLine + "GO" +
          Environment.NewLine + Environment.NewLine
          );
      }

      //Print(stringBuilder.ToString());

      return stringBuilder.ToString();
    }


    internal void ScriptObject(Urn urn, Scripter scripter)
    {
      StringCollection stringCollection = scripter.Script(new Urn[] { urn });
      StringBuilder stringBuilder = new StringBuilder();

      string ObjectFullName = GetFullName(urn);
      string ObjectType = urn.Type;

      if (ScriptingIsInProgress == false)
      {
        stringBuilder.Append("/* It is a bug! This string never appears in this script text !!! */" + Environment.NewLine + Environment.NewLine);
        stringBuilder.Clear();
        ScriptingIsInProgress = true;
      }

      foreach (string str in stringCollection)
      {
        stringBuilder.Append(str + Environment.NewLine + "GO" + Environment.NewLine + Environment.NewLine);
      }

      SaveObjectToDictionary(urn, ObjectFullName, stringBuilder.ToString());
    }


    private void SaveObjectToDictionary(Urn urn, string ObjectFullName, string ScriptText)
    {
      if (Objects.TryGetValue(urn, out string PreviousText)) // Данный объект уже есть в словаре //
      {
        if (PreviousText == ScriptText) // С прошлой проверки не было изменений //
        {
          if (AppSettings.LogUnchangedObject)
          {
            Ms.Message("No changes found", urn.Type + " " + ObjectFullName).NoAlert().Debug();
            OutputDevice.OutputMessage(urn.Type + " " + ObjectFullName + " no changes found.");
          }
        }
        else // Объект изменился //
        {
          // save to file old value = text
          // save to file new value = ScriptText
          SaveObjectToFile(urn, ObjectFullName, PreviousText, false);
          SaveObjectToFile(urn, ObjectFullName, ScriptText, true);
          Objects[urn] = ScriptText;
        }
      }
      else // Данный объект обнаружен первый раз //
      {
        // save to file new value = ScriptText
        SaveObjectToFile(urn, ObjectFullName, ScriptText, true);
        Objects.Add(urn, ScriptText);
      }
    }


    public string ReplaceFirst(string text, string search, string replace)
    {
      int pos = text.IndexOf(search);
      if (pos < 0)
      {
        return text;
      }
      return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
    }


    private void SaveObjectToFile(Urn urn, string ObjectFullName, string ScriptText, bool CurrentObjectValue)
    {
      if ((CurrentObjectValue == false) && (AppSettings.SaveObjectHistory == false)) return;
      string FileExtension = CurrentObjectValue ? "sql" : TTConvert.TimeAsFileName + ".sql";

      string FileName = ObjectFullName;

      if ((AppSettings.UseDatabaseNameInFileName == false) && (urn.Type.ToLower() != "database"))
      {
        FileName = ReplaceFirst(FileName, "[" + AppSettings.DBName + "].", string.Empty);
        FileName = FileName.Replace("[].", string.Empty);
      }

      if (AppSettings.UseSquareBracketsInFileName == false)
      {
        FileName = FileName.Replace("]", string.Empty).Replace("[", string.Empty);
      }


      FileName = FileName + "." + FileExtension;



      string Folder = Path.Combine(AppSettings.MainFolder, "script_" + AppSettings.DBName, urn.Type.Replace(" ", ""));
      FileName = Path.Combine(Folder, Path.GetFileName(FileName));

      try
      {
        if (!Directory.Exists(Folder)) Directory.CreateDirectory(Folder);
        File.WriteAllText(FileName, ScriptText);
      }
      catch (Exception ex)
      {
        OutputDevice.OutputMessage("Ошибка при записи даных в файл!");
        OutputDevice.OutputMessage("Имя файла = " + FileName);
        OutputDevice.OutputMessage(ex.Message);
        OutputDevice.OutputMessage(ex.StackTrace);
        return;
      }

      if (CurrentObjectValue == false) return;

      OutputDevice.OutputMessage(urn.Type + " " + ObjectFullName + " is saved.");
      Ms.Message("Object is saved", urn.Type + " " + ObjectFullName).NoAlert().Debug();

    }


    public void ScriptDatabaseObjects()
    {
      ScriptingIsInProgress = false;

      ServerConnection serverConnection = new ServerConnection(Connection);

      OutputDevice.OutputMessage("{clear}");
      OutputDevice.OutputMessage("Scripting is in progress...");

      try
      {

        Server server = new Server(serverConnection);
        Database db = server.Databases[AppSettings.DBName];
        Scripter scripter = new Scripter(server);

        // scripter.ScriptingProgress += new ProgressReportEventHandler(ScriptingProgressEventHandler);

        if (scriptingOptions == null) throw new Exception("Ошибка! Программист забыл инициализировать объект [scriptingOptions]");

        if (scriptingOptions.TargetServerVersion != AppSettings.MsSqlVersion) scriptingOptions.TargetServerVersion = AppSettings.MsSqlVersion;

        scripter.Options = scriptingOptions;
        //---------------------------------------------------------------------------------------------------------------//
        if (AppSettings.ScriptDatabase)
        {
          ScriptObject(db.Urn, scripter); //stringBuilder.Append(ScriptObject(db.Urn, scripter));
        }
        //---------------------------------------------------------------------------------------------------------------//
        if (AppSettings.ScriptUserTableType)
        {
          foreach (UserDefinedTableType item in db.UserDefinedTableTypes)
          {
            ScriptObject(item.Urn, scripter); Application.DoEvents();
          }
        }
        //---------------------------------------------------------------------------------------------------------------//
        if (AppSettings.ScriptTable)
        {
          server.SetDefaultInitFields(typeof(Table), IsSystemObject);
          foreach (Table item in db.Tables)
          {
            if (!item.IsSystemObject)
            {
              ScriptObject(item.Urn, scripter); Application.DoEvents();
            }
          }
        }
        //---------------------------------------------------------------------------------------------------------------//
        if (AppSettings.ScriptView)
        {
          server.SetDefaultInitFields(typeof(Microsoft.SqlServer.Management.Smo.View), IsSystemObject);

          foreach (Microsoft.SqlServer.Management.Smo.View item in db.Views)
          {
            if (!item.IsSystemObject)
            {
              ScriptObject(item.Urn, scripter); Application.DoEvents();
            }
          }
        }
        //---------------------------------------------------------------------------------------------------------------//
        if (AppSettings.ScriptFunction)
        {
          server.SetDefaultInitFields(typeof(UserDefinedFunction), IsSystemObject);
          foreach (UserDefinedFunction udf in db.UserDefinedFunctions)
          {
            if (!udf.IsSystemObject)
            {
              ScriptObject(udf.Urn, scripter); Application.DoEvents();
            }
          }
        }
        //---------------------------------------------------------------------------------------------------------------//
        if (AppSettings.ScriptProcedure)
        {
          server.SetDefaultInitFields(typeof(StoredProcedure), IsSystemObject);

          foreach (StoredProcedure sp in db.StoredProcedures)
          {
            if (!sp.IsSystemObject)
            {
              ScriptObject(sp.Urn, scripter); Application.DoEvents();
            }
          }
        }
        //---------------------------------------------------------------------------------------------------------------//

        //---------------------------------------------------------------------------------------------------------------//

        //---------------------------------------------------------------------------------------------------------------//

        OutputDevice.OutputMessage("Done.");

        scripter = null;
        db = null;
        server = null;
        serverConnection.Disconnect();

      }
      catch (Exception ex)
      {
        OutputDevice.OutputMessage("An error has occured!");
        OutputDevice.OutputMessage(ex.Message);
        OutputDevice.OutputMessage(ex.StackTrace);
      }
      ScriptingIsInProgress = false;
    }
  }
}
