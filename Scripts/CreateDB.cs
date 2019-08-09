using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDB : MonoBehaviour
{

    DataService ds;
    // Start is called before the first frame update
    void Awake()
    {
        ds = new DataService("Textura.db");
        ds.CreateDB();
    }

    /// <summary>
    /// Guarda el estado de los edificios
    /// </summary>
    public void saveData()
    {
        
        ds.GuardarEdificios();

    }
    /// <summary>
    /// Carga el estado de los edificios
    /// </summary>
    public void LoadData()
    {
       
        ds.CargarEdificios();

    }
}
