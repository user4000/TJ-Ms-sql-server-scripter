using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ProjectStandard
{
  public static class XXRadGridView
  {
    public static string ZZGetStringValue(this RadGridView grid, string GridColumnName)
    {// Получим значение ячейки в указанном столбце текущей строки //
      string s = string.Empty;
      try
      {
        GridViewRowInfo selectedRow = grid.CurrentCell.RowInfo;
        s = Convert.ToString(selectedRow.Cells[GridColumnName].Value); /* Внимание! Имя столбца грида а не имя столбца данных таблицы */
      }
      catch { }
      return s;
    }

    public static string ZZGetStringValueByFieldName(this RadGridView grid, string FieldName) => ZZGetStringValue(grid, TTStandard.GetGridColumnName(FieldName));

    public static void ZZSetGridMainProperties(this RadGridView grid, int RowHeight = 30, bool AllowRowResize = false)
    {
      grid.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
      grid.TableElement.RowHeight = RowHeight;
      grid.AllowRowResize = AllowRowResize;
    }

    public static void ZZLoadColumnWidth(this RadGridView grid, Dictionary<string, int> d)
    {
      int width = 0;
      try
      {
        for (int i = 0; i < grid.ColumnCount; i++)
          if (d.TryGetValue(grid.Columns[i].FieldName, out width))
            grid.Columns[i].Width = width;
      }
      catch { }
    }

    public static void ZZSaveColumnWidth(this RadGridView grid, Dictionary<string, int> d)
    {
      d.Clear();
      for (int i = 0; i < grid.ColumnCount; i++)
        d.Add(grid.Columns[i].FieldName, grid.Columns[i].Width);
    }



    public static GridViewDataColumn ZZGetByFieldName(this RadGridView grid, string FieldName)
    {
      GridViewDataColumn[] collection = grid.Columns.GetColumnByFieldName(FieldName);
      if (collection.Count<GridViewDataColumn>() == 1) return collection.First(); else return null;
    }


    public static int ZZGetColumnWidthByFieldName(this RadGridView grid, string FieldName)
    {
      GridViewDataColumn[] collection = grid.Columns.GetColumnByFieldName(FieldName);
      if (collection.Count<GridViewDataColumn>() == 1) return collection.First().Width; else return -1;
    }

    public static bool ZZIsColumnVisibleByFieldName(this RadGridView grid, string FieldName)
    {
      GridViewDataColumn[] collection = grid.Columns.GetColumnByFieldName(FieldName);
      if (collection.Count<GridViewDataColumn>() == 1) return collection.First().IsVisible; else return false;
    }

    public static int ZZGetRowIndex(this RadGridView grid, string GridColumnName, string Value, bool MakeCurrent)
    {
      int rowIndex = -1; GridViewRowInfo row = null;

      try
      {
        row = grid.Rows
         .Cast<GridViewRowInfo>()
         .Where(r => r.Cells[GridColumnName].Value.ToString().Equals(Value))
         .First();
        rowIndex = row.Index;
      }
      catch { rowIndex = -1; }

      if ((rowIndex >= 0) && MakeCurrent) grid.Rows[rowIndex].IsCurrent = true;

      return rowIndex;
    }

    public static int ZZGetRowIndexForeach(this RadGridView grid, string GridColumnName, string Value, bool MakeCurrent)
    {
      int rowIndex = -1;
      foreach (GridViewRowInfo row in grid.Rows)
      {
        if (row.Cells[GridColumnName].Value.ToString().Equals(Value))
        {
          rowIndex = row.Index;
          break;
        }
      }
      if ((rowIndex >= 0) && MakeCurrent) grid.Rows[rowIndex].IsCurrent = true;
      return rowIndex;
    }



    /*
    public static async Task ZZFilterRefresh(this RadGridView grid)
    {
      grid.EnableFiltering = false;
      var t = Task.Factory.StartNew(() => { Task.Delay(10).Wait(); }); await t;
      grid.EnableFiltering = true;
    }
    */
  }
}

