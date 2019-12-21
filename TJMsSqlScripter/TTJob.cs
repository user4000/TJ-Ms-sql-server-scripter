using System;
using System.Windows.Forms;

namespace MainProject
{

  internal class TTJob
  {

    internal TTPeriod Period { get; set; }

    internal Timer JobTimer { get; set; }

    internal IWorkToExecute Work { get; set; }


    internal void ExecuteWork()
    {
      if (Work is null) return;
      Work.Execute();
    }


    internal void EventTimerTick(object sender, EventArgs e) => ExecuteWork();


    internal void EnableTimerTickEvent(bool Enable)
    {
      if (Enable)
        JobTimer.Tick += EventTimerTick;
      else
        JobTimer.Tick -= EventTimerTick;
    }
    
    internal bool IsTimeToWork(DateTime time)
    {
      DateTime dt1 = new DateTime(time.Year, time.Month, time.Day, Period.Hour1, Period.Minute1, 0);
      DateTime dt2 = new DateTime(time.Year, time.Month, time.Day, Period.Hour2, Period.Minute2, 59);
      return ((dt1 <= time) && (dt2 >= time));
    }
  }
}
