using SQLite4Unity3d;

/// <summary>
/// Enumera los distintos identificadores en el vector
/// </summary>
public class Numeracion
{
    [PrimaryKey]
    public int id { get; set; }
   



    public override string ToString()
    {
        return string.Format("[Numeracion: id={0}]", id);
    }
}