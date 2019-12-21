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
  public class TTDay
  {
    public int Number { get; set; }

    public string  Name { get; set; }

    public bool Checked { get; set; }

  }

  public class TTDayFactory
  {
    public static TTDay Create(DayOfWeek day) => new TTDay() { Number = (int)day, Name = day.ToString(), Checked = false };

    public static void Fill(BindingList<TTDay> list)
    {
      list.Add(TTDayFactory.Create(DayOfWeek.Monday));
      list.Add(TTDayFactory.Create(DayOfWeek.Tuesday));
      list.Add(TTDayFactory.Create(DayOfWeek.Wednesday));
      list.Add(TTDayFactory.Create(DayOfWeek.Thursday));
      list.Add(TTDayFactory.Create(DayOfWeek.Friday));
      list.Add(TTDayFactory.Create(DayOfWeek.Saturday));
      list.Add(TTDayFactory.Create(DayOfWeek.Sunday));
    }

    public static void Bind(RadCheckedListBox listBox, BindingList<TTDay> list)
    {
      listBox.DataSource = list;
      listBox.DisplayMember = nameof(TTDay.Name);
      listBox.ValueMember = nameof(TTDay.Number);
      listBox.CheckedMember = nameof(TTDay.Checked);
    }
  }
}
