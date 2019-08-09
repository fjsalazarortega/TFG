using SQLite4Unity3d;

/// <summary>
/// Clase encargada de gestionar las diversas texturas
/// </summary>
public class Textura
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; } //Identificador de textura
    public string Nombre { get; set; } //Nombre de las texturas
    public int Ancho { get; set; } //Ancho de la imagen
    public int Alto { get; set; } //Alto de la imagen
    public byte[] Image { get; set; } //Imagen como array de bits
    public int Nmipmap { get; set; } //Número de divisiones de la imagen

    public override string ToString()
    {
        return string.Format("[Textura: Id={0}, Nombre={1},  Ancho={2}, Alto={3},Imagen={4}, Mipmap={5}]", Id, Nombre, Ancho, Alto, Image.LongLength, Nmipmap);
    }
}
