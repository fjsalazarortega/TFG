using SQLite4Unity3d;
using UnityEngine;
using System.IO;

using System;
using System.Drawing;


#if !UNITY_EDITOR
using System.Collections;
#endif
using System.Collections.Generic;

public class DataService
{
    private int[] listpow2;
    private string connectionString;
    private SQLiteConnection _connection;
    public DataService(string DatabaseName)
    {
#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
      //  Debug.Log("Final PATH: " + dbPath);

    }
    /// <summary>
    /// Método que crea la base de datos y la rellena.
    /// </summary>
    public void CreateDB()
    {
        ArrayPow2();

        //Creación de tablas si no existen
        _connection.CreateTable<Textura>();
        _connection.CreateTable<Buildings>();
        _connection.CreateTable<Numeracion>();

        try
        {
            if (this.GetTamtablaTexture() == 0)
            {
                //Sí la BBDD está vacia carga las texturas de prueba
                if (Application.platform == RuntimePlatform.Android)
                {
                    BetterStreamingAssets.Initialize();
                    string[] dirs = BetterStreamingAssets.GetFiles("texturasInicial", "*.*");


                    foreach (string nombreimage in dirs)
                    {
                        if (!nombreimage.Contains(".meta") && (nombreimage.Contains(".jpg") || nombreimage.Contains(".png")))
                        {
                            string[] subfile = nombreimage.Split('/');
                            GuardarImagen(BetterStreamingAssets.ReadAllBytes(nombreimage), subfile[1]);
                        }
                    }


                }
                else
                {
                    //analiza si hay nuevas fotos de prueba que añadir y las añade
                    string[] dirs = System.IO.Directory.GetFiles(@"Assets/StreamingAssets/texturasInicial");

                    foreach (string nombreimage in dirs)
                    {
                        if (!nombreimage.Contains(".meta") && (nombreimage.Contains(".jpg") || nombreimage.Contains(".png")))
                        {
                            string[] subfile = nombreimage.Split('\\');

                            GuardarImagen(File.ReadAllBytes(@"Assets/StreamingAssets/texturasInicial/" + subfile[1]), subfile[1]);
                        }
                    }
                }
            }
        }
        catch { Debug.Log("No existe la carpeta TexturaInicial"); }
        return;

    }
    /// <summary>
    ///Devuelve los elementos que tiene la tabla de edificios
    /// </summary>
    /// <returns>Elementos de la tabla</returns>
    public Buildings[] GetBuildings()
    {
        Buildings[] vect = new Buildings[this.GetTamtablaBuildings()];
        int cont = 0;
        foreach (var build in _connection.Table<Buildings>())
        {
            vect[cont] = build;
            cont++;
        }
        return vect;
    }

    /// <summary>
    /// Devuelve los elementos de la tabla texturas
    /// </summary>
    /// <returns>Elementos de la tabla</returns>
    public Textura[] GetTextures()
    {
        Textura[] vect = new Textura[this.GetTamtablaTexture()];
        int cont = 0;
        foreach (var tex in _connection.Table<Textura>())
        {
           
            vect[cont] = tex;
            cont++;
        }
        return vect;
    }

    /// <summary>
    /// Obtiene el valor de la numeración de los nuevos elementos de la tabla y se inserta en la tabla 
    /// </summary>
    /// <returns>tamaño de la tabla Numeración</returns>
    public int Getnumeracion()
    {
        var Numeracion = new Numeracion { id = _connection.Table<Numeracion>().Count() };
        _connection.Insert(Numeracion);
        return _connection.Table<Numeracion>().Count();
    }

    /// <summary>
    /// Muestra mensajes de prueba por pantalla
    /// </summary>
    /// <param name="msg">mensaje a imprimir en pantalla</param>
    private void ToConsole(string msg)
    {
        Debug.Log(msg);
    }


    /// <summary>
    /// Obtiene el tamaño de la tabla de texturas
    /// </summary>
    /// <returns> Número de texturas</returns>
    public int GetTamtablaTexture()
    {
        return _connection.Table<Textura>().Count();
    }

    /// <summary>
    /// Obtiene el tamaño de la tabla de edificios 
    /// </summary>
    /// <returns> número de elementos de la tabla</returns>
    public int GetTamtablaBuildings()
    {

        return _connection.Table<Buildings>().Count();
    }

   
    /// <summary>
    /// Este método guarda los edificios que le han puesto texturas
    /// </summary>
    public void GuardarEdificios()
    {
        try
        {
            Transform[] trans = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
            Dictionary<string, string> map = null;
            foreach (Transform t in trans)
            {
                if (t.gameObject.name == "ButtonscrollList")
                {

                    map = t.gameObject.GetComponent<ButtonListControl>().givemap();
                }

            }
            //Guardamos los edificios que han sido texturizados en el mapa
            foreach (var entry in map)
            {
                var p = new Buildings
                {
                    rutabuild = entry.Key,
                    nameImage = entry.Value

                };
                _connection.InsertOrReplace(p);
            }
        }
        catch
        {
            Debug.Log("No se ha creado correctamente la tabla Buildings");
        }
    }

    /// <summary>
    /// Carga las texturas de los edificios que se guardaron
    /// </summary>
    public void CargarEdificios()
    {
        try
        {
            Buildings[] buildings = this.GetBuildings();
            Transform[] trans = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
            Dictionary<string, string> map = null;
            foreach (Transform t in trans)
            {
                if (t.gameObject.name == "ButtonscrollList")
                {

                    map = t.gameObject.GetComponent<ButtonListControl>().givemap();

                }

            }

            Dictionary<string, string> aux = new Dictionary<string, string>(map);
            if (buildings.Length != 0)
            {
                for (int i = 0; i < buildings.Length; i++)
                {
                    aux.Remove(buildings[i].rutabuild);
                    GameObject gameobject = GameObject.Find(buildings[i].rutabuild);
                    string nameText = buildings[i].nameImage;
                    Textura Textur = _connection.Find<Textura>(d1 => d1.Nombre == nameText);

                    if (Textur != null)
                    {
                        //Se añade la textura a la pared del edificio
                        Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                        texture.LoadImage(Textur.Image);
                        texture.name = Textur.Nombre;
                        gameobject.transform.GetComponent<MeshRenderer>().material.mainTexture = texture;
                    }
                    else
                    {
                        //en el caso de que no haya textura asociada (techo) se vuelve amarillo
                        Renderer color = gameobject.GetComponent<Renderer>();
                        color.material.color = UnityEngine.Color.yellow;
                    }
                }
                //en el caso de que no deba cargarse esta textura (techo por que no haya nada) volverlo a poner blanco
                GameObject cube = GameObject.Find("Cube");
                foreach (var entry in aux)
                {
                    GameObject go = GameObject.Find(entry.Key);
                    go.transform.GetComponent<MeshRenderer>().material.mainTexture = cube.transform.GetComponent<MeshRenderer>().material.mainTexture;
                    Renderer color = go.GetComponent<Renderer>();
                    color.material.color = UnityEngine.Color.white;
                }


            }

        }
        catch { Debug.Log("No hay datos de los edificios"); }
    }

/// <summary>
/// Añade las texturas de prueba a la tabla
/// </summary>
/// <param name="data"> Array de bytes con la informacion de la imagen</param>
/// <param name="nombre">Nombre de la imagen</param>
    public void GuardarImagen(byte[] data, string nombre)
    {
        try
        {

            Texture2D bit = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            bit.LoadImage(data);

            //Comprobamos que este mismo elemento no esté en la tabla y no lo añade

            Pair<Pair<int, int>, Pair<int, int>> anchoxalto = Potenciade2(bit.width, bit.height);
            string[] separador = nombre.Split('.');
            string nombrecorrecto = separador[0];
          
            if (this.TexturaEnBaseDeDatos(nombrecorrecto) == null)
            {

                var p = new Textura
                {
                    Nombre = nombrecorrecto,
                    Ancho = bit.width,
                    Alto = bit.height,
                    Image = data,
                    //se guarda el numero de veces que se ha hecho mipmapping
                    Nmipmap = Math.Min(anchoxalto.Second.First, anchoxalto.Second.Second)
                };
                _connection.Insert(p);
                MipMapping(bit, nombrecorrecto, anchoxalto.First.First, anchoxalto.First.Second);
            }
        }
        catch
        {
            Debug.Log("fallo en la insercción de fotos");
        }
    }




    /// <summary>
    /// Crea un mipmapping de la textura y la guarda en la BBDD
    /// </summary>
    /// <param name="orig">Textura original</param>
    /// <param name="nombre">Nombre de la imagen original </param>
    /// <param name="ancho">Ancho que debe tener la nueva imagen</param>
    /// <param name="alto">Alto que debe tener la nueva imagen</param>
    private void MipMapping(Texture2D orig, string nombre, int ancho, int alto)
    {
        //hacemos mipmapping
        //comprobamos si alguna de las imagenes mipmapping han sido borradas para restaurarlas
        string nombreaux = nombre;

        while (ancho > 64 && alto > 64)
        {
            nombreaux = nombre + " " + ancho.ToString() + " x " + alto.ToString();

            byte[] data;
            TextureScale.Point(orig, ancho, alto);
            data = orig.EncodeToJPG();

            var p = new Textura
            {
                Nombre = nombreaux,
                Ancho = ancho,
                Alto = alto,
                Image = data,
                Nmipmap = -1
            };
            _connection.Insert(p);
            alto = alto / 2;
            ancho = ancho / 2;
        }
    }

    public Textura TexturaEnBaseDeDatos(string name)
    {
        try
        {
            var textura = _connection.Find<Textura>(d1 => d1.Nombre == name);
            return textura;
        }
        catch {
            Debug.Log("fallo");
            return null;
        }
       
    }
    /// <summary>
    ///Devuelve la potencia de dos mas cercana a las pasadas
    /// </summary>
    /// <param name="widht">Ancho de la imagen</param>
    /// <param name="heigh">Alto de la imagen</param>
    /// <returns> devuelve un par con las potencias de dos y la posicion de estas </returns>
    private Pair<Pair<int, int>, Pair<int, int>> Potenciade2(int widht, int heigh)
    {
        ArrayPow2();
        Pair<int, int> a = new Pair<int, int>();
        Pair<int, int> b = new Pair<int, int>();
        Pair<Pair<int, int>, Pair<int, int>> valor = new Pair<Pair<int, int>, Pair<int, int>>();
        valor.First = a;
        valor.Second = b;
        bool secon_power_width = false;
        bool secon_power_height = false;

        for (int i = 0; i < listpow2.Length; i++)
        {
            if (!secon_power_width && listpow2[i] >= widht)
            {

                valor.First.First = listpow2[i - 1];
                secon_power_width = true;
                valor.Second.First = i;
            }

            if (!secon_power_height && listpow2[i] >= heigh)
            {

                valor.First.Second = listpow2[i - 1];
                secon_power_height = true;
                valor.Second.Second = i;
            }
        }
        return valor;
    }

    /// <summary>
    /// Rellena el array de potencia de dos
    /// </summary>
    private void ArrayPow2()
    {
        listpow2 = new int[8];
        listpow2[0] = 64;
        listpow2[1] = 128;
        listpow2[2] = 256;
        listpow2[3] = 512;
        listpow2[4] = 1024;
        listpow2[5] = 2048;
        listpow2[6] = 4096;
        listpow2[7] = 8192;
    }

}
