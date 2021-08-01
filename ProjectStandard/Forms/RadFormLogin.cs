using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Threading.Tasks;
using TJFramework;
using static TJFramework.TJFrameworkManager;

namespace ProjectStandard
{
  public partial class RadFormLogin : RadForm, IEventStartWork
  {
    private string TxtStatusOn { get; } = "Соединение установлено";
    private string TxtStatusOff { get; } = "Соединение не установлено";

    private string TxtConnect { get; } = "Подключиться";
    private string TxtDisconnect { get; } = "Отключиться";

    private const int PasswordMinimumLength = 5;

    private Timer TimerEnableButtonConnect = new Timer();

    public string LoginText
    {
      get { return F_Login.Text; }
      set { F_Login.Text = value; }
    }

    public string PasswordText
    {
      get { return F_Password.Text; }
      set { F_Password.Text = value; F_Password.SelectAll(); }
    }

    public ILoginFormManager LoginFormManager { get; set; } = null;

    public void InitLoginFormManager(ILoginFormManager manager)
    {
      LoginFormManager = manager;
      if (LoginFormManager == null) { LoginFormManagerIsNull(); return; }
      LoginFormManager.Disconnect();
      LoginText = LoginFormManager.LastLogin();
    }

    public RadFormLogin()
    {
      InitializeComponent();
      Start();
    }

    private void Start()
    {
      SetTimer();
      SetProperties();
      SetEvents();

      BtnChangePassword.Enabled = false;
      PageViewLogin.SelectedPage = PageLogin;
    }

    private void SetProperties()
    {
      PageLogin.Item.MinSize = new System.Drawing.Size(200, 0);
      PagePassword.Item.MinSize = new System.Drawing.Size(200, 0);
      PageLogin.Item.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
      PagePassword.Item.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
    }

    private void SetEvents()
    {
      if (PictureOn.Location != PictureOff.Location) { PictureOn.Location = PictureOff.Location; }
      this.Shown += (s, e) => ShowConnectionIndicator(false, false);
      this.BtnConnect.Click += async (s, e) => await ConnectButton();
      this.BtnChangePassword.Click += async (s, e) => await EventChangePassword();
      TxtOld.TextChanged += EventUserChangedPasswordSymbols;
      TxtNew1.TextChanged += EventUserChangedPasswordSymbols;
      TxtNew2.TextChanged += EventUserChangedPasswordSymbols;

      PicQuestion.Click += EventPictureTooltipClick;
    }
    private void EventPictureTooltipClick(object sender, EventArgs e)
    {
      string h = "Подсказка";
      string s = "Кнопка [Сменить пароль] станет активной \r\nесли будут соблюдены все правила смены пароля." +
        "\r\n1. Введён текущий пароль" +
        $"\r\n2. Введён новый пароль (не менее {PasswordMinimumLength} символов)" +
        "\r\n3. Повторно введённый новый пароль совпадает с указанным выше.";
      Ms.Message(MsgType.Info, h, s, PicQuestion,  MsgPos.Unknown, 20).Create();
    }
    private void LoginFormManagerIsNull() => Ms.Message(MsgType.Error, "Ошибка!", "Не инициализирован объект типа [ILoginFormManager]", BtnChangePassword, MsgPos.Unknown, 4).Create();

    private async Task EventChangePassword()
    {
      if (LoginFormManager == null) { LoginFormManagerIsNull(); return; }

      bool flag = true;
      if ((flag) && (TxtOld.Text.Length < 1))
      {
        Ms.Message(MsgType.Warning, "Ошибка!", "Не указан ваш текущий пароль", BtnChangePassword,  MsgPos.Unknown, 4).Create();
        flag = false;
      }
      if ((flag) && (TxtNew1.Text != TxtNew2.Text))
      {
        Ms.Message(MsgType.Warning, "Ошибка!", "Новый пароль не совпадает с повторным вводом.", BtnChangePassword, MsgPos.Unknown, 4).Create();
        flag = false;
      }
      if ((flag) && (TxtNew1.Text.Length < PasswordMinimumLength))
      {
        Ms.Message(MsgType.Warning, "Ошибка!", $"Новый пароль не должен быть менее {PasswordMinimumLength}", BtnChangePassword,  MsgPos.Unknown, 4).Create();
        flag = false;
      }

      TTReturnCode code = await LoginFormManager.ChangePassword(F_Login.Text, TxtOld.Text, TxtNew1.Text);

      if (code.ReturnCode == 0)
      {
        TxtNew1.Clear(); TxtNew2.Clear(); TxtOld.Clear();
        await ConnectButton();
        PageViewLogin.SelectedPage = PageLogin;
        Ms.Message(MsgType.Ok, "Изменение пароля", "Пароль успешно изменён. Войдите в систему заново с новым паролем.", F_Login, MsgPos.Unknown, 4).Create();
      }
      else
        Ms.Message(MsgType.Error, "Ошибка при попытке смены пароля.", code.ReturnMessage, TxtOld, MsgPos.Unknown, 4).Create();
    }

    private void EventUserChangedPasswordSymbols(object sender, EventArgs e)
    {
      BtnChangePassword.Enabled =
        (TxtNew1.Text == TxtNew2.Text) &&
        (TxtNew1.Text.Length > 4) &&
        (TxtOld.Text.Length > 0);
    }

    private void SetTimer()
    {
      TimerEnableButtonConnect.Interval = 1000;
      TimerEnableButtonConnect.Enabled = false;
      TimerEnableButtonConnect.Tick += (s, e) => TimerTick();
    }

    private void TimerTick()
    {
      BtnConnect.Enabled = true;
      TimerEnableButtonConnect.Enabled = false;
      F_Password.Clear();
    }

    public async Task ConnectButton()
    {
      BtnConnect.Enabled = false;
      if (LoginFormManager == null) { LoginFormManagerIsNull(); return; }
      if (LoginFormManager.IsConnected == false)
      {
        await LoginFormManager.Connect(F_Login.Text, F_Password.Text, PanelLogin);
      }
      else
      {
        await LoginFormManager.Disconnect();
      }

      ShowConnectionIndicator(LoginFormManager.IsConnected);
      TimerEnableButtonConnect.Enabled = true;
    }

    private void ShowConnectionIndicator(bool Connected, bool SetPanelCaption = true)
    {
      PictureOn.Visible = Connected; PictureOff.Visible = !Connected;
      BtnConnect.Text = Connected ? TxtDisconnect : TxtConnect;
      F_Login.Enabled = !Connected;
      F_Password.Enabled = !Connected;
      PagePassword.Enabled = Connected;
      if (PictureOn.Location != PictureOff.Location) PictureOn.Location = PictureOff.Location;
      if (SetPanelCaption) { PanelLogin.Text = Connected ? TxtStatusOn : TxtStatusOff; }
    }

    private async Task EventPasswordKeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter) await ConnectButton();
    }

    private void SetPropertiesOfPasswordTextBox()
    {
      this.ActiveControl = F_Password;
      //F_Password.KeyUp += EventPasswordKeyUp;
      F_Password.KeyUp += async (s, e) => await EventPasswordKeyUp(s, e);
    }


    public void EventStartWork()
    {
      SetPropertiesOfPasswordTextBox();
    }
  }
}