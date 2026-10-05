//using UnityEngine;
//using UnityEngine.SceneManagement;
//using TMPro;

//public class UIHUDMinijuego : MonoBehaviour
//{
//    public static UIHUDMinijuego Instancia;

//    [Header("Texto / Vidas")]
//    public TMP_Text textoVidas;

//    [Header("Paneles de Juego")]
//    public GameObject panelGameOver;
//    public GameObject panelVictoria;

//    [Header("Nombres de Escenas")]
//    [Tooltip("Nombre exacto de la escena del Menú Principal")]
//    public string nombreEscenaMenu = "MainMenu";

//    private void Awake()
//    {
//        if (Instancia == null)
//        {
//            Instancia = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void Start()
//    {
//        Time.timeScale = 1f; // Restaurar la velocidad normal del juego

//        if (panelGameOver != null) panelGameOver.SetActive(false);
//        if (panelVictoria != null) panelVictoria.SetActive(false);
//    }

//    public void ActualizarVidas(int cantidadVidas)
//    {
//        if (textoVidas != null)
//        {
//            textoVidas.text = $"Vidas: {cantidadVidas}";
//        }
//    }

//    public void MostrarGameOver()
//    {
//        if (panelGameOver != null) panelGameOver.SetActive(true);
//        Time.timeScale = 0f; // Pausa el minijuego
//    }

//    public void MostrarVictoria()
//    {
//        if (panelVictoria != null) panelVictoria.SetActive(true);
//        Time.timeScale = 0f; // Pausa el minijuego
//    }

//    // --- Métodos para los Botones de la UI ---

//    public void ReintentarMinijuego()
//    {
//        Time.timeScale = 1f;
//        string escenaActual = SceneManager.GetActiveScene().name;
//        SceneManager.LoadScene(escenaActual);
//    }

//    public void VolverAlMenu()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(nombreEscenaMenu);
//    }
//}

//using UnityEngine;
//using UnityEngine.SceneManagement;
//using TMPro;

//public class UIHUDMinijuego : MonoBehaviour
//{
//    public static UIHUDMinijuego Instancia;

//    [Header("Texto / Vidas")]
//    public TMP_Text textoVidas;

//    [Header("Paneles de Juego")]
//    public GameObject panelGameOver;
//    public GameObject panelVictoria;

//    [Header("Nombres de Escenas")]
//    [Tooltip("Nombre exacto de la escena del Menú Principal")]
//    public string nombreEscenaMenu = "MainMenu";

//    private void Awake()
//    {
//        if (Instancia == null)
//        {
//            Instancia = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//            return;
//        }

//        // Apagar paneles inmediatamente en Awake para evitar que se muestren un solo frame
//        OcultarPaneles();
//    }

//    private void Start()
//    {
//        Time.timeScale = 1f; // Restaurar la velocidad normal del juego
//        OcultarPaneles();
//    }

//    private void OcultarPaneles()
//    {
//        if (panelGameOver != null) panelGameOver.SetActive(false);
//        if (panelVictoria != null) panelVictoria.SetActive(false);
//    }

//    public void ActualizarVidas(int cantidadVidas)
//    {
//        if (textoVidas != null)
//        {
//            textoVidas.text = $"Vidas: {cantidadVidas}";
//        }
//    }

//    public void MostrarGameOver()
//    {
//        if (panelGameOver != null) panelGameOver.SetActive(true);
//        Time.timeScale = 0f; // Pausa el minijuego
//    }

//    public void MostrarVictoria()
//    {
//        if (panelVictoria != null) panelVictoria.SetActive(true);
//        Time.timeScale = 0f; // Pausa el minijuego
//    }

//    // --- Métodos para los Botones de la UI ---

//    public void ReintentarMinijuego()
//    {
//        Time.timeScale = 1f;
//        string escenaActual = SceneManager.GetActiveScene().name;
//        SceneManager.LoadScene(escenaActual);
//    }

//    public void VolverAlMenu()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(nombreEscenaMenu);
//    }
//}

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIHUDMinijuego : MonoBehaviour
{
    public static UIHUDMinijuego Instancia;

    [Header("Texto / Vidas")]
    public TMP_Text textoVidas;

    [Header("Paneles de Juego")]
    public GameObject panelGameOver;
    public GameObject panelVictoria;

    //[Header("Nombres de Escenas")]
    //[Tooltip("Nombre exacto de la escena del Menú Principal")]
    //public string nombreEscenaMenu = "Menu";

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        OcultarPaneles();
    }

    private void Start()
    {
        Time.timeScale = 1f; // Asegura que el juego empiece a velocidad normal
        OcultarPaneles();
    }

    private void OcultarPaneles()
    {
        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (panelVictoria != null) panelVictoria.SetActive(false);
    }

    public void ActualizarVidas(int cantidadVidas)
    {
        if (textoVidas != null)
        {
            textoVidas.text = $"Vidas: {cantidadVidas}";
        }
    }

    public void MostrarGameOver()
    {
        if (panelGameOver != null) panelGameOver.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego
    }

    public void MostrarVictoria()
    {
        if (panelVictoria != null) panelVictoria.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego
    }

    // --- Métodos para los Botones de la UI ---

    public void ReintentarMinijuego()
    {
        Debug.Log("¡BOTÓN REINTENTAR PRESIONADO!");
        Time.timeScale = 1f; // RESTAURAR TIEMPO ANTES DE CARGAR
        string escenaActual = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(escenaActual);

    }

    public void VolverAlMenu()
    {
        Debug.Log("¡BOTÓN volver PRESIONADO!");
        Time.timeScale = 1f; // RESTAURAR TIEMPO ANTES DE CARGAR
        SceneManager.LoadScene("Menu");
    }
}