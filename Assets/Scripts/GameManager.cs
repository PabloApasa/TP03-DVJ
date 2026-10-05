//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using UnityEngine.SceneManagement;

//public enum ModoJuego
//{
//    Aprendizaje,
//    Minijuego
//}

//public class GameManager : MonoBehaviour
//{
//    [System.Serializable]
//    public struct ConfigSena
//    {
//        public string nombreSena;
//        public Sprite imagenSena;
//    }

//    [Header("Configuración de Señas")]
//    public List<ConfigSena> listaSenas;
//    private int indiceSenaActual = 0;

//    [Header("Referencias de UI")]
//    public TMP_Text textoProfesora;
//    public TMP_Text textoFeedback;
//    public UnityEngine.UI.Image imagenDisplaySena;

//    [Header("Profesora")]
//    public ProfesoraTTS profesora;

//    [Header("Jugador del minijuego")]
//    public JugadorMinijuego jugadorMinijuego;

//    [Header("Estado del Juego")]
//    [HideInInspector] public string senaActualTarget = "";

//    private bool esperandoSena = true;

//    [Header("Modo de juego")]
//    public ModoJuego modoJuego = ModoJuego.Aprendizaje;

//    // -----------------------------
//    // CONTROL DE DETECCIÓN
//    // -----------------------------

//    [Header("Control de detección")]
//    [Tooltip("Cantidad de frames consecutivos necesarios para aceptar una seña")]
//    public int framesNecesarios = 8;

//    [Tooltip("Tiempo que esperamos al cargar una nueva seña antes de reconocer")]
//    public float tiempoPreparacion = 1.0f;

//    private int framesCorrectos = 0;
//    private float tiempoInicioSena;

//    // Control para evitar la repetición continua de saltos
//    private bool accionEjecutada = false;


//    void Start()
//    {
//        if (modoJuego == ModoJuego.Aprendizaje)
//        {
//            if (listaSenas != null && listaSenas.Count > 0)
//            {
//                CargarSenaActual();
//            }
//            else
//            {
//                Debug.LogError("¡Atención! La lista de señas está vacía en el Inspector.");
//            }
//        }
//    }


//    void Update()
//    {
//        // Teclas de prueba para testing en editor
//        if (Input.GetKeyDown(KeyCode.Alpha1) && esperandoSena)
//        {
//            OnSenaDetectada("ILOVEYOU");
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha2) && esperandoSena)
//        {
//            OnSenaDetectada("NO");
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha3) && esperandoSena)
//        {
//            OnSenaDetectada("FAMILIA");
//        }
//    }


//    // ==========================================================
//    // MEDIA PIPE
//    // ==========================================================

//    public void ProcesarLandmarksMediaPipe(Vector2[] puntos)
//    {
//        if (puntos == null || puntos.Length < 21)
//        {
//            // Si la mano sale de pantalla, liberamos el bloqueo de salto
//            accionEjecutada = false;
//            return;
//        }

//        // Modo Minijuego
//        if (modoJuego == ModoJuego.Minijuego)
//        {
//            ProcesarSenasMinijuego(puntos);
//            return;
//        }

//        // Modo Aprendizaje
//        if (!esperandoSena)
//            return;

//        if (senaActualTarget == "ILOVEYOU" && EvaluadorSenas.EsILoveYou(puntos))
//        {
//            OnSenaDetectada("ILOVEYOU");
//        }
//        else if (senaActualTarget == "NO" && EvaluadorSenas.EsSenaNo(puntos))
//        {
//            OnSenaDetectada("NO");
//        }
//        else if (senaActualTarget == "FAMILIA" && EvaluadorSenas.EsFamilia(puntos))
//        {
//            OnSenaDetectada("FAMILIA");
//        }
//    }


//    private void ProcesarSenasMinijuego(Vector2[] puntos)
//    {
//        bool esILoveYou = EvaluadorSenas.EsILoveYou(puntos);
//        bool esNo = EvaluadorSenas.EsSenaNo(puntos);
//        bool esFamilia = EvaluadorSenas.EsFamilia(puntos);

//        // 1. Si detecta una seña y no hemos saltado aún en esta oportunidad:
//        if ((esILoveYou || esNo || esFamilia) && !accionEjecutada)
//        {
//            accionEjecutada = true;

//            if (esILoveYou)
//            {
//                if (jugadorMinijuego != null) jugadorMinijuego.Saltar(); // Salto 1
//                Debug.Log("I LOVE YOU detectado → SALTO 1");
//            }
//            else if (esNo)
//            {
//                if (jugadorMinijuego != null) jugadorMinijuego.Saltar2(); // Salto 2
//                Debug.Log("NO detectado → SALTO 2");
//            }
//            else if (esFamilia)
//            {
//                if (jugadorMinijuego != null) jugadorMinijuego.Saltar3(); // Salto 3
//                Debug.Log("FAMILIA detectado → SALTO 3");
//            }
//        }
//        // 2. Si relajas la mano o dejas de hacer las señas válidas:
//        else if (!esILoveYou && !esNo && !esFamilia)
//        {
//            // Reseteamos la bandera para poder hacer el próximo salto
//            accionEjecutada = false;
//        }
//    }


//    // ==========================================================
//    // CARGAR NUEVA SEÑA (MODO APRENDIZAJE)
//    // ==========================================================

//    private void CargarSenaActual()
//    {
//        esperandoSena = true;
//        framesCorrectos = 0;
//        tiempoInicioSena = Time.time;

//        ConfigSena actual = listaSenas[indiceSenaActual];
//        senaActualTarget = actual.nombreSena;

//        if (textoProfesora != null)
//        {
//            textoProfesora.text = $"Haz la seña: <b>{senaActualTarget}</b>";
//        }

//        if (textoFeedback != null)
//        {
//            textoFeedback.text = "";
//        }

//        if (imagenDisplaySena != null && actual.imagenSena != null)
//        {
//            imagenDisplaySena.sprite = actual.imagenSena;
//        }

//        if (profesora != null)
//        {
//            profesora.DarInstruccion();
//        }

//        Debug.Log($"[NUEVA SEÑA] Ahora toca: {senaActualTarget}");
//    }


//    // ==========================================================
//    // SEÑA DETECTADA
//    // ==========================================================

//    private void OnSenaDetectada(string senaDetectada)
//    {
//        if (!esperandoSena)
//            return;

//        esperandoSena = false;
//        framesCorrectos = 0;

//        if (textoFeedback != null)
//        {
//            textoFeedback.text = "<color=green>¡CORRECTO!</color>";
//        }

//        if (profesora != null)
//        {
//            profesora.Siguiente();
//        }

//        Debug.Log($"[CORRECTO] Seña detectada: {senaDetectada}");

//        StartCoroutine(RutinaSiguienteSena());
//    }

//    public void ReiniciarAccion()
//    {
//        accionEjecutada = false;
//    }


//    // ==========================================================
//    // SIGUIENTE SEÑA
//    // ==========================================================

//    private IEnumerator RutinaSiguienteSena()
//    {
//        yield return new WaitForSeconds(2.0f);

//        indiceSenaActual++;

//        if (indiceSenaActual < listaSenas.Count)
//        {
//            CargarSenaActual();
//        }
//        else
//        {
//            if (textoProfesora != null)
//            {
//                textoProfesora.text = "¡Felicidades! Has completado todas las señas.";
//            }

//            if (textoFeedback != null)
//            {
//                textoFeedback.text = "<color=yellow>¡Nivel Completado!</color>";
//            }

//            if (profesora != null)
//            {
//                profesora.Finalizar();
//            }

//            Debug.Log("[JUEGO] ¡Todas las señas completadas!");

//            StartCoroutine(CambiarDeEscena());
//        }
//    }

//    private IEnumerator CambiarDeEscena()
//    {
//        yield return new WaitForSeconds(2f);

//        int escenaActual = SceneManager.GetActiveScene().buildIndex;
//        SceneManager.LoadScene(escenaActual + 1);
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public enum ModoJuego
{
    Aprendizaje,
    Minijuego
}

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct ConfigSena
    {
        public string nombreSena;
        public Sprite imagenSena;
    }

    [Header("Configuración de Señas")]
    public List<ConfigSena> listaSenas;
    private int indiceSenaActual = 0;

    [Header("Referencias de UI")]
    public TMP_Text textoProfesora;
    public TMP_Text textoFeedback;
    public UnityEngine.UI.Image imagenDisplaySena;

    [Header("Profesora")]
    public ProfesoraTTS profesora;

    [Header("Jugador del minijuego")]
    public JugadorMinijuego jugadorMinijuego;

    [Header("Estado del Juego")]
    [HideInInspector] public string senaActualTarget = "";

    private bool esperandoSena = true;

    [Header("Modo de juego")]
    public ModoJuego modoJuego = ModoJuego.Aprendizaje;

    // -----------------------------
    // CONTROL DE DETECCIÓN
    // -----------------------------

    [Header("Control de detección")]
    [Tooltip("Cantidad de frames consecutivos necesarios para aceptar una seña")]
    public int framesNecesarios = 8;

    [Tooltip("Tiempo que esperamos al cargar una nueva seña antes de reconocer")]
    public float tiempoPreparacion = 1.0f;

    private int framesCorrectos = 0;
    private float tiempoInicioSena;

    // Control para evitar la repetición continua de saltos
    private bool accionEjecutada = false;


    void Start()
    {
        if (modoJuego == ModoJuego.Aprendizaje)
        {
            if (listaSenas != null && listaSenas.Count > 0)
            {
                CargarSenaActual();
            }
            else
            {
                Debug.LogError("¡Atención! La lista de señas está vacía en el Inspector.");
            }
        }
    }


    void Update()
    {
        // Teclas de prueba para testing en editor
        if (Input.GetKeyDown(KeyCode.Alpha1) && esperandoSena)
        {
            OnSenaDetectada("ILOVEYOU");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && esperandoSena)
        {
            OnSenaDetectada("NO");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && esperandoSena)
        {
            OnSenaDetectada("FAMILIA");
        }
    }


    // ==========================================================
    // MEDIA PIPE
    // ==========================================================

    public void ProcesarLandmarksMediaPipe(Vector2[] puntos)
    {
        if (puntos == null || puntos.Length < 21)
        {
            // Si la mano sale de pantalla, liberamos el bloqueo de salto
            accionEjecutada = false;
            return;
        }

        // Modo Minijuego
        if (modoJuego == ModoJuego.Minijuego)
        {
            ProcesarSenasMinijuego(puntos);
            return;
        }

        // Modo Aprendizaje
        if (!esperandoSena)
            return;

        if (senaActualTarget == "ILOVEYOU" && EvaluadorSenas.EsILoveYou(puntos))
        {
            OnSenaDetectada("ILOVEYOU");
        }
        else if (senaActualTarget == "NO" && EvaluadorSenas.EsSenaNo(puntos))
        {
            OnSenaDetectada("NO");
        }
        else if (senaActualTarget == "FAMILIA" && EvaluadorSenas.EsFamilia(puntos))
        {
            OnSenaDetectada("FAMILIA");
        }
    }


    private void ProcesarSenasMinijuego(Vector2[] puntos)
    {
        bool esILoveYou = EvaluadorSenas.EsILoveYou(puntos);
        bool esNo = EvaluadorSenas.EsSenaNo(puntos);
        bool esFamilia = EvaluadorSenas.EsFamilia(puntos);

        // 1. Si detecta una seña y no hemos saltado aún en esta oportunidad:
        if ((esILoveYou || esNo || esFamilia) && !accionEjecutada)
        {
            accionEjecutada = true;

            if (esILoveYou)
            {
                if (jugadorMinijuego != null) jugadorMinijuego.Saltar2(); // Salto 2
                Debug.Log("I LOVE YOU detectado → SALTO 2");
            }
            else if (esNo)
            {
                if (jugadorMinijuego != null) jugadorMinijuego.Saltar(); // Salto 1
                Debug.Log("NO detectado → SALTO 1");
            }
            else if (esFamilia)
            {
                if (jugadorMinijuego != null) jugadorMinijuego.Saltar3(); // Salto 3
                Debug.Log("FAMILIA detectado → SALTO 3");
            }
        }
        // 2. Si relajas la mano o dejas de hacer las señas válidas:
        else if (!esILoveYou && !esNo && !esFamilia)
        {
            // Reseteamos la bandera para poder hacer el próximo salto
            accionEjecutada = false;
        }
    }


    // ==========================================================
    // CARGAR NUEVA SEÑA (MODO APRENDIZAJE)
    // ==========================================================

    private void CargarSenaActual()
    {
        esperandoSena = true;
        framesCorrectos = 0;
        tiempoInicioSena = Time.time;

        ConfigSena actual = listaSenas[indiceSenaActual];
        senaActualTarget = actual.nombreSena;

        if (textoProfesora != null)
        {
            textoProfesora.text = $"Haz la seña: <b>{senaActualTarget}</b>";
        }

        if (textoFeedback != null)
        {
            textoFeedback.text = "";
        }

        if (imagenDisplaySena != null && actual.imagenSena != null)
        {
            imagenDisplaySena.sprite = actual.imagenSena;
        }

        if (profesora != null)
        {
            profesora.DarInstruccion();
        }

        Debug.Log($"[NUEVA SEÑA] Ahora toca: {senaActualTarget}");
    }


    // ==========================================================
    // SEÑA DETECTADA
    // ==========================================================

    private void OnSenaDetectada(string senaDetectada)
    {
        if (!esperandoSena)
            return;

        esperandoSena = false;
        framesCorrectos = 0;

        if (textoFeedback != null)
        {
            textoFeedback.text = "<color=green>¡CORRECTO!</color>";
        }

        if (profesora != null)
        {
            profesora.Siguiente();
        }

        Debug.Log($"[CORRECTO] Seña detectada: {senaDetectada}");

        StartCoroutine(RutinaSiguienteSena());
    }

    public void ReiniciarAccion()
    {
        accionEjecutada = false;
    }


    // ==========================================================
    // SIGUIENTE SEÑA
    // ==========================================================

    private IEnumerator RutinaSiguienteSena()
    {
        yield return new WaitForSeconds(2.0f);

        indiceSenaActual++;

        if (indiceSenaActual < listaSenas.Count)
        {
            CargarSenaActual();
        }
        else
        {
            if (textoProfesora != null)
            {
                textoProfesora.text = "¡Felicidades! Has completado todas las señas.";
            }

            if (textoFeedback != null)
            {
                textoFeedback.text = "<color=yellow>¡Nivel Completado!</color>";
            }

            if (profesora != null)
            {
                profesora.Finalizar();
            }

            Debug.Log("[JUEGO] ¡Todas las señas completadas!");

            StartCoroutine(CambiarDeEscena());
        }
    }

    private IEnumerator CambiarDeEscena()
    {
        yield return new WaitForSeconds(2f);

        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(escenaActual + 1);
    }
}