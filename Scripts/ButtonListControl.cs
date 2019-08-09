using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Se encarga de controlar el listado de todas las texturas
/// </summary>
public class ButtonListControl : MonoBehaviour
{
    [SerializeField]
    public GameObject ImageTemplate; //texturas originales
    public GameObject ImageTemplate2; // texturas mipmmaping
    private Dictionary<string, string> map; // arbol que organiza las texturas colocadas en los edificios
    Textura[] arrayText; // vector que contiene todas las texturas


    private void Start()
    {
        //leemos todas las texturas de las fotos originales
        map = new Dictionary<string, string>();
        string nombre = this.name;
        if ("ButtonscrollList" == this.name)
        {
            DataService ds = new DataService("Textura.db");
            arrayText = ds.GetTextures();
            for (int i = 0; i < ds.GetTamtablaTexture(); i = i + arrayText[i].Nmipmap)//solo se cogen originales
            {
                GameObject image = Instantiate(ImageTemplate) as GameObject;
                image.name = arrayText[i].Id.ToString();
                image.SetActive(true);

                //se coloca la foto en la posición que les corresponda
                image.transform.SetParent(ImageTemplate.transform.parent, false);
                Texture2D prueba = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                prueba.LoadImage(arrayText[i].Image);
                image.transform.GetComponent<RawImage>().texture = prueba;
            }
        }
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Añade fotos al menú de texturas
    /// </summary>
    /// <param name="valor_anterior"></param>
    /// <param name="ds"></param>
    public void AñadirFoto(int valor_anterior, DataService ds)
    {

        arrayText = ds.GetTextures();
        Transform[] trans = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
        GameObject button = null;
        //busca el menu de texturas y lo activa unos momentos para poder crear la nueva textura
        foreach (Transform t in trans)
        {
            if (t.gameObject.name == "ButtonscrollList"){
               button = t.gameObject;
               button.SetActive(true);
            }
            if (t.gameObject.name == "Menu_texturas"){
 
                t.gameObject.SetActive(true);
            }
        }
        //añade la foto
        GameObject image = Instantiate(ImageTemplate) as GameObject;
        image.name = arrayText[valor_anterior].Id.ToString();
        image.SetActive(true);

        //Se coloca en su sitio
        image.transform.SetParent(ImageTemplate.transform.parent, false);
        Texture2D prueba = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        prueba.LoadImage(arrayText[valor_anterior].Image);
        image.transform.GetComponent<RawImage>().texture = prueba;

        button.SetActive(false);
    }

    /// <summary>
    /// abre el menú mipmapping de la textura tocada
    /// </summary>
    /// <param name="text">id de la textura</param>
    public void ImagenClick(string text)
    {

        int id = int.Parse(text) - 1;
        int mip = arrayText[id].Nmipmap;
        Transform[] trans = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
       
        //se abre el menú mipmmaping
        foreach (Transform t in trans)
        {
            if (t.gameObject.name == "ButtonscrollList2"){

                t.gameObject.SetActive(true);
                break;

            }
        }
        GameObject submenu = GameObject.Find("ButtonscrollList2");
        // busca las fotos correspondientes a la textura seleccionadas
        for (int i = id + 1; i < id + mip; i++)
        {

            GameObject image = Instantiate(ImageTemplate2) as GameObject;
            image.name = arrayText[i].Id.ToString();
            image.SetActive(true);

            image.transform.SetParent(ImageTemplate2.transform.parent, false);
            Texture2D prueba = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            prueba.LoadImage(arrayText[i].Image);

            image.transform.GetComponent<RawImage>().texture = prueba;

        }
        this.gameObject.SetActive(false);

    }

    /// <summary>
    /// Se encarga de poner una textura a la pared correspondiente o no hace nada
    /// </summary>
    /// <param name="text"></param>
    public void ImagenClick2(string text)
    {
        int id = int.Parse(text) - 1;
        //Debug.Log(arrayText[id].Nombre);
        Texture2D texture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
        //carga la imagen
        texture.LoadImage(arrayText[id].Image);
        texture.name = arrayText[id].Nombre;
        GameObject camara = GameObject.Find("FirstPersonCharacter");
        PonerTextura p = camara.GetComponent<PonerTextura>();
        if (p.obtenerObjeto() != null){

            GameObject objeto1 = p.obtenerObjeto();
            //se añade textura
            objeto1.transform.GetComponent<MeshRenderer>().material.mainTexture = texture;
            string name = objeto1.name;
            GameObject house = null;

            //busca si el elementos seleccionado es un edificio
            while (objeto1.transform.parent != null)
            {
                if (objeto1.tag == "edificios")
                {
                    house = objeto1;
                }
                objeto1 = objeto1.transform.parent.gameObject;
                name = objeto1.name + "/" + name;

            }

            //se guarda en el mapa para posterior guardado o borrado
            if (map.ContainsKey(name))
            {
                map.Remove(name);
                map.Add(name, arrayText[id].Nombre);
            }
            else
            {
                map.Add(name, arrayText[id].Nombre);
            }

            Transform[] trans = house.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trans)
            {
                //en el caso de que se restaure el estado del mundo, se elimina las texturas de los edificios previamente no guardados
                if (t.gameObject.tag == "Techo")
                {
                    string name2 = t.name;
                    GameObject aux = t.gameObject;
                    Renderer color = t.GetComponent<Renderer>();
                    color.material.color = Color.yellow;
                    while (aux.transform.parent != null)
                    {

                        aux = aux.transform.parent.gameObject;
                        name2 = aux.name + "/" + name2;

                    }
                    if (map.ContainsKey(name2))
                    {
                        map.Remove(name2);
                        map.Add(name2, "yellow");
                    }
                    else
                    {
                        map.Add(name2, "yellow");
                    }
                }
            }
        }
    }
    /// <summary>
    /// Mapa con los distintos edificio que han modificado la textura
    /// </summary>
    /// <returns>Mapa con la textura de los edificios </returns>
    public Dictionary<string, string> givemap()
    {
        return map;
    }
}
