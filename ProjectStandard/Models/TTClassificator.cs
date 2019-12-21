using System;

namespace Model
{
  [Serializable]
  public class TTClassificator 
  {
    public int IdObject { get; set; }
    public int IdParent { get; set; }
    public string CodeObject { get; set; }
    public int RankObject { get; set; }
    public string NameShort { get; set; }
    public string NameObject { get; set; }
    public string NoteObject { get; set; }
  }
}
