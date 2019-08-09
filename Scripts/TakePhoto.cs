using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class TakePhoto : MonoBehaviour
{

    public ButtonListControl Control;
    public RawImage imagenCam;
    private WebCamTexture webCamTexture;
    private string fileName = "foto";
    private string Ruta;
    private DataService ds;
    private GameObject Menu_aviso;
    private GameObject m_image;
    private byte[] bytes;
    private Texture2D captura;

    // Start is called before the first frame update
    void Start()
    {
        //Se asigna los diferentes elementos
        Transform[] trans = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trans)
        {
            if (t.gameObject.name == "Menu_aviso")
            {
                Menu_aviso = t.gameObject;
            }
        }
        ds = new DataService("Textura.db");
        webCamTexture = new WebCamTexture();
        imagenCam.texture = webCamTexture;
        webCamTexture.Play();
    }

    /// <summary>
    /// Guarda una imagen realizada por la cámara
    /// </summary>
    public void GuardaImagenCam()
    {
        try
        {
            //Se recogen las dimensiones de la foto y se guarda la imagen en JPG
            captura = new Texture2D(webCamTexture.width, webCamTexture.height);
            captura.SetPixels(webCamTexture.GetPixels());
            captura.Apply();
            bytes = captura.EncodeToJPG();

            //Se guarda la imagen en el vector de texturas
            int valor = ds.GetTamtablaTexture();
            ds.GuardarImagen(bytes, fileName + ds.Getnumeracion() + ".jpg");
            Control.AñadirFoto(valor, ds);
        }
        catch { Debug.Log("fallo"); }
    }

    /// <summary>
    /// Obtiene las imagenes que esten en la carpeta Imagen del movil
    /// </summary>
    public void ObtenerDeImage()
    {
        string path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("/Android")) + "/Imagen";
        BetterStreamingAssets.Initialize();

        if (Application.platform == RuntimePlatform.Android)
        {
            if (!System.IO.Directory.Exists(path))
            {
                
                System.IO.Directory.CreateDirectory(path);
                string[] dirs = BetterStreamingAssets.GetFiles("Otratextura", "*.*");
                foreach (string nombreimage in dirs)
                {
                    if (!nombreimage.Contains(".meta"))
                    {
                        //Se almacena la imagen de prueba
                        string[] subfile = nombreimage.Split('/');
                        Texture2D ss = new Texture2D(2, 2);
                        byte[] b = BetterStreamingAssets.ReadAllBytes(nombreimage);
                        ss.LoadImage(b);
                        File.WriteAllBytes(path + "/Final.png", b);
                        File.OpenRead(path + "/Final.png");
                        //Se fuerza un escaneo de la imagen de prueba
                        using (AndroidJavaClass jcUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                        using (AndroidJavaObject joActivity = jcUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                        using (AndroidJavaObject joContext = joActivity.Call<AndroidJavaObject>("getApplicationContext"))
                        using (AndroidJavaClass jcMediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection"))
                        using (AndroidJavaClass jcEnvironment = new AndroidJavaClass("android.os.Environment"))
                        using (AndroidJavaObject joExDir = jcEnvironment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory"))
                        {
                            jcMediaScannerConnection.CallStatic("scanFile", joContext, new string[] { path+"/Final.png" }, null, null);
                        }
                    }
                }
                //aviso de carpeta creada
                Menu_aviso.SetActive(true);
                GameObject aviso = GameObject.Find("Aviso");
                aviso.GetComponent<Text>().text = "Agrege las fotos a la carpeta Imagen en tu galería y vuelve a pulsar";
            }
            else
            {
                string[] dirs = System.IO.Directory.GetFiles(path);

                if (dirs.Length == 1)
                {
                    //no hay fotos que añadir
                    Menu_aviso.SetActive(true);
                    GameObject aviso = GameObject.Find("Aviso");
                    aviso.GetComponent<Text>().text = "No hay fotos que agregar";
                }
                else
                {
                    bool ninguna_nueva = true;
                    foreach (string nombreimage in dirs)
                    {
                        if (!nombreimage.Contains(".meta") && (nombreimage.Contains(".png") || nombreimage.Contains(".jpg")) && !nombreimage.Contains("Final.png"))
                        {
                            //Se comprueba si las fotos han sido ya guardadas en la BBDD
                            string[] sub = nombreimage.Split('/');
                            string[] separador = sub[sub.Length - 1].Split('.');
                            string nombrecorrecto = separador[0];
                            Textura textura = ds.TexturaEnBaseDeDatos(nombrecorrecto);
                            if (textura == null) {
                                //si no llega aqui es que no habia nuevas fotos que añadir
                                ninguna_nueva = false;
                                byte[] b = File.ReadAllBytes(nombreimage);
                                int valor = ds.GetTamtablaTexture();

                                ds.GuardarImagen(b,  sub[sub.Length - 1].ToString());
                                Control.AñadirFoto(valor, ds);
                            }
                        }
                    }

                    Menu_aviso.SetActive(true);
                    GameObject aviso = GameObject.Find("Aviso");
                    if (!ninguna_nueva)
                    {
                        aviso.GetComponent<Text>().text = "Se han añadido al menú de textura";
                    }
                    else {
                        aviso.GetComponent<Text>().text = "No habia ninguna nueva foto que añadir";
                    }
                }
            }
        }
    }
}

