using System.Data;
using System.Threading.Tasks;
using Telerik.WinControls.UI;
using System.Linq;
using System.Windows.Forms;

namespace ProjectStandard
{
  public abstract class TTGridWithDataTable : TTGridManager // Действия с объектом который представлен в табличном виде. Содержит RadGridView связанный с DataTable //
  {
    public BindingSource DataBindingSource { get; } = new BindingSource();

    public DataTable Table { get; set; } = new DataTable();

    public TTGridWithDataTable() { } // Constructor //

    public void SetDataSourceForGrid() => Grid.DataSource = DataBindingSource;

    public delegate Task TypeDelegateEventDataColumnChanging(object sender, DataColumnChangeEventArgs e);

    private TypeDelegateEventDataColumnChanging DelegateEventDataColumnChanging { get; set; } = null;

    protected bool DelegateEventDataColumnChangingEnabled { get; set; } = true;

    public override void InitializeGrid(RadGridView grid, bool SetAnotherTheme = false) // Called from User Form Constructor //
    {
      base.InitializeGrid(grid, SetAnotherTheme);
      DataBindingSource.DataSource = Table;
      SetDataSourceForGrid();
      Table.ColumnChanging += async (s, e) => await EventDataColumnChanging(s, e);
    }

    public void SetEventDataColumnChanging(TypeDelegateEventDataColumnChanging method)
    {
      DelegateEventDataColumnChanging = method;
    }

    protected async Task EventDataColumnChanging(object sender, DataColumnChangeEventArgs e)
    { // Событие: Пользователь закончил редактирование содержимого ячейки //
      string CurrentColumnName = e.Column.ColumnName;
      string CurrentCellValue = e.Row[CurrentColumnName].ToString();
      string ProposedCellValue = e.ZzProposedValue();

      //MessageBox.Show($"{CurrentColumnName}---{CurrentCellValue}---{ProposedCellValue}","EventDataColumnChanging");
      //ms.Debug( $"{CurrentColumnName}---{CurrentCellValue}---{ProposedCellValue}", "EventDataColumnChanging");

      if ( // if column cannot be empty we return old value //
          (ProposedCellValue.Trim().Length < 1) &&
          (ColumnCannotBeEmpty(CurrentColumnName))
         )
        e.ProposedValue = CurrentCellValue;

      if ( (DelegateEventDataColumnChanging != null) && DelegateEventDataColumnChangingEnabled ) await DelegateEventDataColumnChanging.Invoke(sender, e);
    }

    public DataRow FindRowByKey(string KeyField, string KeyValue) => Table.Select($"{KeyField}={KeyValue}").FirstOrDefault();

    public void UpdateOneCell(string KeyField, string KeyValue, string FieldName, string FieldValue)
    {
      DataRow row = FindRowByKey(KeyField, KeyValue);
      if (row != null)
      {
        DelegateEventDataColumnChangingEnabled = false;
        row[FieldName] = FieldValue;
        DelegateEventDataColumnChangingEnabled = true;
      }
    }
  }
}
