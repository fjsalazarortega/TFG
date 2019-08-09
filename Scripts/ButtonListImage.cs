using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// Clase encargada de gestionar que foto ha sido pulsada
/// </summary>
public class ButtonListImage : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private ButtonListControl imageControl;


    /// <summary>
    /// Se encarga de analizar el toque de la textura
    /// </summary>
    /// <param name="eventData"> Textura que se analiza</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        //Se encarga de saber que elemento ha sido pulsado en el menú de texturas o mipmapping
        GameObject submenu = GameObject.Find("ButtonscrollList2");
        if (submenu==null)
        {
            imageControl.ImagenClick(name);
        }
        else
        {
            imageControl.ImagenClick2(name);
        }
    }
}
