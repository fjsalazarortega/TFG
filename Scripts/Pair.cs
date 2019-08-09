/// <summary>
/// Clase que gestiona dos elementos 
/// </summary>
/// <typeparam name="T">Elemento 1</typeparam>
/// <typeparam name="U">Elemento 2</typeparam>
public class Pair<T, U>
{
    public Pair()
    {
    }

    public Pair(T first, U second)
    {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }
};
