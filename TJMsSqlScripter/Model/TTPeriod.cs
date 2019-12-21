using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace MainProject
{
  [Serializable]
  public class TTPeriod
  {
    public int Number { get; set; }
    public byte Hour1 { get; set; }
    public byte Minute1 { get; set; }
    public byte Hour2 { get; set; }
    public byte Minute2 { get; set; }
    public int IntervalMinute { get; set; }
  }

  public class TTPeriodService
  {
    public static TTPeriod Create(byte hour1, byte minute1, byte hour2, byte minute2, int intervalMinute)
    {
      TTPeriod timer = new TTPeriod()
      {
        Number = 0,
        Hour1 = hour1,
        Minute1 = minute1,
        Hour2 = hour2,
        Minute2 = minute2,
        IntervalMinute = intervalMinute
      };
      return timer;
    }

    public static TTPeriod Create(DateTime dt1, DateTime dt2, int intervalMinute)
    {
      TTPeriod timer = new TTPeriod()
      {
        Number = 0,
        Hour1 = (byte)dt1.Hour,
        Minute1 = (byte)dt1.Minute,
        Hour2 = (byte)dt2.Hour,
        Minute2 = (byte)dt2.Minute,
        IntervalMinute = intervalMinute
      };
      return timer;
    }

    public static void SortByStartTime(BindingList<TTPeriod> list)
    {
      BindingList<TTPeriod> SortedList = new BindingList<TTPeriod>(list.OrderBy(o => ( o.Hour1*100 + o.Minute1 )).ToList());

      int j = 0;
      foreach(TTPeriod item in SortedList)
      {
        j++; item.Number = j;
      }

      list = SortedList;
    }

    public static bool AddToList(BindingList<TTPeriod> list, TTPeriod timer)
    {
      timer.Number = list.Count + 1;

      foreach (TTPeriod item in list) if (Intersection(item, timer)) return false;

      list.Add(timer);

      SortByStartTime(list);

      return true;
    }

    public static DateTime Date1(TTPeriod timer) => new DateTime(2019, 07, 29, timer.Hour1, timer.Minute1, 0);

    public static DateTime Date2(TTPeriod timer) => new DateTime(2019, 07, 29, timer.Hour2, timer.Minute2, 0);

    public static bool Intersection(DateTime AxStart, DateTime AxEnd, DateTime BxStart, DateTime BxEnd) =>
      ((AxStart <= BxStart) && (AxEnd >= BxStart)) || ((BxStart <= AxStart) && (BxEnd >= AxStart));


    public static bool Intersection(TTPeriod t1, TTPeriod t2) =>
      Intersection(Date1(t1), Date2(t1), Date1(t2), Date2(t2));


    public static void Bind(RadGridView grid, BindingList<TTPeriod> list, bool Bind)
    {
      if (Bind)
        grid.DataSource = list;
      else
        grid.DataSource = null;
    }

  }
}
