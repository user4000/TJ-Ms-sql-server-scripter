using System.ComponentModel;
using System.Windows.Forms;

namespace MainProject
{
  internal class TTJobFactory
  {
    internal TTJob Create(IContainer container, TTPeriod period, IWorkToExecute work )
    {
      TTJob job = new TTJob();
      job.JobTimer = new Timer(container);
      job.Period = period;
      job.JobTimer.Interval = period.IntervalMinute * 60000;
      job.JobTimer.Enabled = false;
      job.Work = work;
      job.EnableTimerTickEvent(true); 
      return job;
    }

    internal void Dispose(TTJob job)
    {
      job.JobTimer.Enabled = false;
      job.EnableTimerTickEvent(false);
      job.JobTimer.Dispose();
      job.Work = null;
      job.JobTimer = null;
      job.Period = null;
    }
  }
}
