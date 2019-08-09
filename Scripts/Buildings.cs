using SQLite4Unity3d;

/// <summary>
/// Clase que constituye un edificio
/// </summary>
public class Buildings 
{
    [PrimaryKey]
    public string rutabuild { get; set; } //ruta del edificio
    public string nameImage { get; set; } //nombre de la textura añadida en él
    
    public override string ToString()
    {
        return string.Format("[Textura: rutabuild={0},  nameImage={1}]", rutabuild,nameImage);
    }
}
