using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class vistazenital : MonoBehaviour
{
    private bool cambio;
    private Camera camara_principal;
    private Camera camara_secundaria;
    private GameObject guia;
    private GameObject player;
    private Canvas control;
    // Start is called before the first frame update
    void Start()
    {
        //Se asignan los valores iniciales
        cambio = false;
        camara_secundaria = GameObject.Find("Zenital").GetComponent<Camera>();
        camara_secundaria.enabled = false;
        camara_principal = GameObject.Find("Camera").GetComponent<Camera>();
        guia = GameObject.Find("Guia");
        guia.SetActive(false);
        player = GameObject.Find("FPSController");
        control = GameObject.Find("DualTouchControls").GetComponent<Canvas>();
    }

    //it is called every fixed frame-rate frame.
    private void FixedUpdate()
    {
        //se realiza un raycast para comprobar si se puede trasladar al usuario con la vista cenital
        if (cambio)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Debug.DrawRay(camara_secundaria.ScreenPointToRay(Input.mousePosition).origin, new Vector3(0,-500,0), Color.red, 20.0f);
                if (Physics.Raycast(camara_secundaria.ScreenPointToRay(Input.mousePosition), out hit, 500f,1 << 8) && hit.transform.tag == "Terrain")
                {
                    player.transform.position = new Vector3(hit.point.x, player.transform.position.y, hit.point.z);
                    
                }

            }
        }
    }
    /// <summary>
    /// Cambia de vista dependiendo si está en modo normal o cenital
    /// </summary>
   public void Cambio_vista() {
        if (!cambio)
        {
            camara_secundaria.enabled = true;
            camara_principal.enabled = false;
            cambio = true;
            guia.SetActive(true);
            control.enabled = false;
        }
        else {
            camara_secundaria.enabled = false;
            camara_principal.enabled = true;
            cambio = false;
            guia.SetActive(false);
            control.enabled = true;
        }
    }
    /// <summary>
    /// Comprueba si esta activado el modo de cámara cenital
    /// </summary>
    /// <returns>devuelve un booleano que indica si se está usando la cámara en modo cenital</returns>
    public bool getCambio() {

        return cambio;
    }
}
