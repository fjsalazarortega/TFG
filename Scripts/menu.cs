using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class menu : MonoBehaviour
{
    private GameObject ButtonscrollList;
    private GameObject Menu_text;
    private GameObject Opciones;
    private GameObject Menu_Opciones;
    private GameObject Anadir;
    private GameObject Menu_anadir;
    private GameObject Menu_aviso;

    /// <summary>
    /// Mantiene un enlace con los distintos elementos necesarios al empezar la ejecución
    /// </summary>
    public void Start()
    {
        //Carga de elementos
        ButtonscrollList = GameObject.Find("ButtonscrollList");
        Menu_text = GameObject.Find("Menu_texturas");
        Opciones = GameObject.Find("Opciones");
        Menu_Opciones = GameObject.Find("Menu_Opciones");
        Menu_Opciones.SetActive(false);
        Anadir = GameObject.Find("Anadir");
        Menu_anadir = GameObject.Find("Menu_anadir");
        Menu_anadir.SetActive(false);
        Menu_aviso = GameObject.Find("Menu_aviso");
        Menu_aviso.SetActive(false);
       
    }
    /// <summary>
    /// Activa el menú de texturas
    /// </summary>
    public void AwakeMenu()
    {
        ButtonscrollList.SetActive(true);
        Menu_text.SetActive(false);
              
               
    }
    /// <summary>
    /// Activa el menú de opciones
    /// </summary>
    public void AwakeMenuOption()
    {      
                Opciones.SetActive(false);
                Menu_Opciones.SetActive(true);
    }

    /// <summary>
    /// Activa el menú para añadir texturas
    /// </summary>
    public void AwakeMenuAnadirTextura()
    {
                Anadir.SetActive(false);
                Menu_anadir.SetActive(true);
            
    }

    /// <summary>
    /// Desactiva el menú de Texturas
    /// </summary>
    public void CloseMenu()
    { 
                ButtonscrollList.SetActive(false);
                Menu_text.SetActive(true);  
    }

    /// <summary>
    /// Desactiva el menú de opciones
    /// </summary>
    public void CloseMenuOption()
    {

               
                Menu_Opciones.SetActive(false);
                Opciones.SetActive(true);
   
    }

    /// <summary>
    /// Desactiva el menú de añadir texturas
    /// </summary>
    public void CloseMenuAnadirTextura()
    {
                Menu_anadir.SetActive(false);
                Anadir.SetActive(true);
      
    }

    /// <summary>
    /// Cierra el diálogo de aviso
    /// </summary>
    public void CloseMenuAviso()
    {
               
                Menu_aviso.SetActive(false);          
    }

    //Cierra el menú con las texturas en mipmapeadas
    public void CloseSubMenu()
    {

        GameObject menu = GameObject.Find("ButtonscrollList2");
        GameObject content = GameObject.Find("ButtonListContent2");
        int hijos = content.transform.childCount;
        for (int i = hijos - 1; i >= 0; i--)
        {
            if (content.transform.GetChild(i).name != "element2")
            {
                GameObject.Destroy(content.transform.GetChild(i).gameObject);

            }

        }
        menu.SetActive(false);
        ButtonscrollList.SetActive(true);
        return;
    }
}
