using System;

namespace Model
{
  [Serializable]
  public class TTOneCell
  {
    public int IdObject { get; set; }
    public string EntityName { get; set; }
    public string ColumnName { get; set; }
    public string Value { get; set; }
  }

}
