using UnityEngine;

public class PonerTextura : MonoBehaviour
{
    private GameObject objeto;
    private int toques = 0;
    private double segundos = 0;



    // Update is called once per frame
    void Update()
    {
        //cuenta el numero de toques que se le da a la pantalla en un intervalo de tiempo
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            toques++;
        }
        if (toques > 0)
        {
            segundos += Time.deltaTime;
        }
        if (toques >= 2)
        {
            Detectado();
            segundos = 0.0f;
            toques = 0;
        }
        if (segundos > 0.5f)
        {
            segundos = 0f;
            toques = 0;
        }
    }

    //Detecta el nombre del objeto que esté tocando y si contiene un tag específico lo prepara para ponerle textura
    void Detectado()
    {


        RaycastHit hit;
        if (objeto != null)
        {
            //Deselecciona el elemento previamente pulsado
            Renderer color1 = objeto.GetComponent<Renderer>();
            color1.material.color = Color.white;
        }

        //Compruebo si se ha clicado un edificio mediante un rayo
        if (!this.GetComponent<vistazenital>().getCambio())
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50f, 1 << 8) && hit.transform.tag == "Pared")
            {
                objeto = hit.collider.gameObject;
                Renderer color = objeto.GetComponent<Renderer>();
                color.material.color = Color.yellow;
            }

        }
    }
    /// <summary>
    /// Objeto, al cual, se le va a modificar la textura
    /// </summary>
    /// <returns>devuelve una pared del edificio </returns>
    public GameObject obtenerObjeto()
    {

        return objeto;
    }
}
