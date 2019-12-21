using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ProjectStandard
{
  public static class XXRadDropDownList
  {
    public static int ConstItemHeight { get; } = 25;

    public static int ConstVisibleItemCount { get; } = 10;
    
    public static int ConstSizeAddition { get; } = 10;

    public static int ConstMaxWidth { get; } = 500;

    public static int ConstMinWidth { get; } = 200;


    public static void ZzSetStandardVisualStyle( this RadDropDownList DDList, SizingMode mode = SizingMode.None )
    {
      DDList.DropDownStyle = RadDropDownStyle.DropDownList;
      DDList.DropDownListElement.ListElement.Font = DDList.Font;
      DDList.DropDownSizingMode = mode;
      DDList.PopupOpening += EventPopupOpening;
    }

    public static string ZzGetStringValueTest(this RadDropDownList DDList) => // TODO: this does not work
      DDList.SelectedValue == null ? string.Empty : DDList.SelectedValue as string;

    public static string ZzGetStringValue(this RadDropDownList DDList) => 
      DDList.SelectedValue as string ?? string.Empty ;

      
    public static int ZzGetIntegerValue(this RadDropDownList DDList) =>
      Convert.ToInt32(DDList.SelectedValue); 

    public static void EventPopupOpening(object sender, CancelEventArgs e)
    {
      if (!(sender is RadDropDownListElement)) return;

      RadDropDownListElement list = sender as RadDropDownListElement; 

      float width = 0;
      for (int x = 0; x < list.Items.Count; x++)
      {
        width = Math.Max(width, TextRenderer.MeasureText(list.Items[x].Text, list.Font).Width);
      }
      if (list.Items.Count * (list.ItemHeight - 1) > list.DropDownHeight)
      {
        width += list.ListElement.VScrollBar.Size.Width;
      }

      list.Popup.Width = Math.Min( (int)width + ConstSizeAddition, ConstMaxWidth );
      if (list.Popup.Width < ConstMinWidth) list.Popup.Width = ConstMinWidth;
      if (list.Popup.Width < list.TextBox.Size.Width) list.Popup.Width = list.TextBox.Size.Width + list.ArrowButton.Size.Width;

      list.ListElement.ItemHeight = ConstItemHeight; 
      list.Popup.Height = Math.Min(list.Items.Count, ConstVisibleItemCount) * (ConstItemHeight) + ConstSizeAddition;

    }
  }
}
