using System;
using System.ComponentModel;
using Telerik.WinControls.UI;
using Microsoft.SqlServer.Management.Smo;
using TJFramework.ApplicationSettings;

namespace MainProject 
{
  internal class TTProjectSettings : TJStandardApplicationSettings
  {
    [Category("База данных")]
    [DisplayName("Имя сервера")]
    public string DBServerName { get; set; } = "DELL5050";

    [Category("База данных")]
    [DisplayName("Имя базы данных")]
    public string DBName { get; set; } = "";

    [Category("База данных")]
    [DisplayName("Имя пользователя")]
    public string DBLastLogin { get; set; } = "";

    [Category("База данных")]
    [DisplayName("Последний успешный вход")]
    public string DBLastSuccessLogin { get { return GetDateTime(HiddenDBLastSuccessLogin); } }


    [Category("Настройки подключения")]
    [DisplayName("Шаблон строки подключения")]
    [Browsable(false)]
    public string DBConnectionTemplate { get; set; } = "Data Source=<SERVER>;Initial Catalog=<DATABASE>;Persist Security Info=True;User ID=<LOGIN>;Password=<PASSWORD>;MultipleActiveResultSets=true;";


    [Category("Настройки приложения")]
    [DisplayName("Автоматический вход в БД после старта приложения")]
    public bool AutoConnectToDb { get; set; } = true;


    [Category("Генерация кода объектов БД")]
    [RadSortOrder(1)]
    [DisplayName("Версия СУБД")]
    public SqlServerVersion MsSqlVersion { get; set; } = SqlServerVersion.Version120;


    [Category("Генерация кода объектов БД")]
    [RadSortOrder(2)]
    [DisplayName("Папка для сохранения скриптов")]
    [ReadOnly(true)]
    public string MainFolder { get; set; } = string.Empty;


    [Category("Генерация кода объектов БД")]
    [RadSortOrder(3)]
    [DisplayName("Сохранять предыдущие версии")]
    public bool SaveObjectHistory { get; set; } = true;



    [Category("Генерация кода объектов БД")]
    [RadSortOrder(4)]
    [DisplayName("Показывать сообщение, если объект не изменился")]
    public bool LogUnchangedObject { get; set; } = true;


    [Category("Генерация кода объектов БД")]
    [RadSortOrder(5)]
    [DisplayName("Указывать имя БД в имени файла скрипта")]
    public bool UseDatabaseNameInFileName { get; set; } = true;


    [Category("Генерация кода объектов БД")]
    [RadSortOrder(6)]
    [DisplayName("Указывать квадратные скобки в имени файла скрипта")]
    public bool UseSquareBracketsInFileName { get; set; } = true;








    [Category("Объекты БД")]
    [RadSortOrder(1)]
    [DisplayName("База данных")]
    public bool ScriptDatabase { get; set; } = false;



    [Category("Объекты БД")]
    [RadSortOrder(2)]
    [DisplayName("User Table Type")]
    public bool ScriptUserTableType { get; set; } = false;


    [Category("Объекты БД")]
    [RadSortOrder(3)]
    [DisplayName("Таблицы")]
    public bool ScriptTable { get; set; } = true;



    [Category("Объекты БД")]
    [RadSortOrder(4)]
    [DisplayName("Представления")]
    public bool ScriptView { get; set; } = true;


    [Category("Объекты БД")]
    [RadSortOrder(5)]
    [DisplayName("Функции")]
    public bool ScriptFunction { get; set; } = true;


    [Category("Объекты БД")]
    [RadSortOrder(6)]
    [DisplayName("Процедуры")]
    public bool ScriptProcedure { get; set; } = true;































    [Browsable(false)]
    public DateTime HiddenDBLastSuccessLogin = DateTime.Today.AddYears(-50);


    [Browsable(false)]
    public string LastIdSession { get; set; } = ""; // <--- Это свойство содержит сохранённый пароль. Название не соответствует содержимиму !!! --- ///


    [Browsable(false)]
    public BindingList<TTDay> ListDays { get; set; } = new BindingList<TTDay>();
  

    [Browsable(false)]
    public BindingList<TTPeriod> ListPeriod { get; set; } = new BindingList<TTPeriod>();






    public override void EventAfterSaving() { }
    public override void EventBeforeSaving() { }
    public override void PropertyValueChanged(string property_name)
    {
      
    }
   }
 }



  /*
   
    EXAMPLE OF ATTRIBUTES

    [Serializable]
    public class MySettings : TJStandardUserSettings
    {
      [Category("Category 1")]
      public string MyString1 { get; set; } = "Privet 1111";

      [Category("Category 1")]
      public DateTime MyDatetime1 { get; set; } = DateTime.Now;

      [Category("Category 2")]
      public Font MyFont1 { get; set; } = new Font("Verdana", 14F, FontStyle.Italic);

      [Category("Category 2")]
      public Color MyColor1 { get; set; } = Color.LightGreen;

      [Category("File Location Example")]
      [Editor(typeof(PropertyGridBrowseEditor), typeof(BaseInputEditor))] // File name dialog //
      public string FileLocation1 { get; set; }

      [Category("Range Example")]
      [RadRange(1, 5)]
      public byte DoorsCount { get; set; } = 4;


      [Browsable(false)]
      public int MyHiddenProperty { get; set; } = 890110000;

      [Category("Read Only Example")]
      [ReadOnly(true)]
      public int Count { get; set; } = 18991;

      [Category("Visible Name differs from Property Name")]
      [DisplayName("This is visible name of a property!")]
      public string PropertyName { get; set; }

      [Category("Description of property example")]
      [Description("The manufacturer of the item")]
      public string Manufacturer { get; set; }

      [Category("Login")]
      [RadSortOrder(0)]
      public string ServerName { get; set; } = "MyServer";

      [Category("Login")]
      [RadSortOrder(1)]
      [Description("User account")]
      public string Username { get; set; } = "Vasya";

      [Category("Login")]
      [RadSortOrder(2)]
      [Description("User password")]
      [PasswordPropertyText(true)]
      public string Password { get; set; } = "";

      [Category("Login"), RadSortOrder(3)]
      public bool Connect { get; set; } = false;

      [Category("Login")]
      [RadSortOrder(4)]
      [ReadOnly(true)]
      [DisplayName("Connection state")]
      public bool ConnectionState { get; set; }


      public DateTime inner_Date_Time = TJStandardDateTimeDefaultValue;

      [Category("Login")]
      [RadSortOrder(5)]
      public string MyDateTime
      {
        get { return GetDateTime(inner_Date_Time); }
        set { inner_Date_Time = SetDateTime(value, inner_Date_Time); }
      }

      [Category("Login")]
      [RadSortOrder(6)]
      public MyEnum my_enum { get; set; } = MyEnum.e_Three;


      public override void PropertyValueChanged(string property_name)
      {
        //Ms.Message(MsgType.Info, property_name, "Changed!", 4,MessagePosition.pos_SC); 
      }


      public override void EventBeforeSaving()
      {
        Password = "";
        //MessageBox.Show("EventBefore_Saving");
      }

      public override void EventAfterSaving()
      {
        Password = "12345";
        //MessageBox.Show("EventAfter_Saving");
      }
    }

  */


