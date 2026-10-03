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
                Debug.LogError(
                    "¡Atención! La lista de señas está vacía en el Inspector."
                );
            }
        }
    }


    void Update()
    {
        // ----------------------------------
        // TECLAS DE PRUEBA
        // ----------------------------------

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
            return;

        // ==========================================
        // MODO MINIJUEGO
        // ==========================================

        if (modoJuego == ModoJuego.Minijuego)
        {
            ProcesarSenasMinijuego(puntos);
            return;
        }


        // ==========================================
        // MODO APRENDIZAJE
        // ==========================================

        if (!esperandoSena)
            return;


        float dIndice = puntos[8].magnitude;
        float dMedio = puntos[12].magnitude;
        float dMenique = puntos[20].magnitude;

        Debug.Log(
            $"[DATOS MANO] Índice: {dIndice:F2} | " +
            $"Medio: {dMedio:F2} | " +
            $"Meñique: {dMenique:F2}"
        );


        if (senaActualTarget == "ILOVEYOU" &&
            EvaluadorSenas.EsILoveYou(puntos))
        {
            OnSenaDetectada("ILOVEYOU");
        }
        else if (senaActualTarget == "NO" &&
                 EvaluadorSenas.EsSenaNo(puntos))
        {
            OnSenaDetectada("NO");
        }
        else if (senaActualTarget == "FAMILIA" &&
                 EvaluadorSenas.EsFamilia(puntos))
        {
            OnSenaDetectada("FAMILIA");
        }
    }
    private bool accionEjecutada = false;

    private void ProcesarSenasMinijuego(Vector2[] puntos)
    {
        // I LOVE YOU → SALTAR
        if (!accionEjecutada &&
            EvaluadorSenas.EsILoveYou(puntos))
        {
            accionEjecutada = true;

            if (jugadorMinijuego != null)
            {
                jugadorMinijuego.Saltar();
            }

            Debug.Log("I LOVE YOU detectado → SALTO");
        }
        // NO → SALTAR2

        if (!accionEjecutada &&
            EvaluadorSenas.EsSenaNo(puntos))
        {
            accionEjecutada = true;

            if (jugadorMinijuego != null)
            {
                jugadorMinijuego.Saltar2();
            }

            Debug.Log("NO detectado → SALTO2");
        }

        // FAMILIA → SALTAR3

        if (!accionEjecutada &&
            EvaluadorSenas.EsFamilia(puntos))
        {
            accionEjecutada = true;

            if (jugadorMinijuego != null)
            {
                jugadorMinijuego.Saltar3();
            }

            Debug.Log("FAMILIA detectado → SALTO3");
        }
    }

    // ==========================================================
    // CARGAR NUEVA SEÑA
    // ==========================================================

    private void CargarSenaActual()
    {
        esperandoSena = true;

        framesCorrectos = 0;

        // Guardamos el momento en que apareció la nueva seña
        tiempoInicioSena = Time.time;


        ConfigSena actual = listaSenas[indiceSenaActual];

        senaActualTarget = actual.nombreSena;


        // ----------------------------------
        // TEXTO
        // ----------------------------------

        if (textoProfesora != null)
        {
            textoProfesora.text =
                $"Haz la seña: <b>{senaActualTarget}</b>";
        }


        // ----------------------------------
        // FEEDBACK
        // ----------------------------------

        if (textoFeedback != null)
        {
            textoFeedback.text = "";
        }


        // ----------------------------------
        // IMAGEN
        // ----------------------------------

        if (imagenDisplaySena != null && actual.imagenSena != null)
        {
            imagenDisplaySena.sprite = actual.imagenSena;
        }


        // ----------------------------------
        // VOZ
        // ----------------------------------

        if (profesora != null)
        {
            profesora.DarInstruccion();
        }


        Debug.Log(
            $"[NUEVA SEÑA] Ahora toca: {senaActualTarget}"
        );
    }


    // ==========================================================
    // SEÑA DETECTADA
    // ==========================================================

    private void OnSenaDetectada(string senaDetectada)
    {
        // Seguridad adicional
        if (!esperandoSena)
            return;


        esperandoSena = false;

        framesCorrectos = 0;


        // ----------------------------------
        // FEEDBACK
        // ----------------------------------

        if (textoFeedback != null)
        {
            textoFeedback.text =
                "<color=green>¡CORRECTO!</color>";
        }


        // ----------------------------------
        // VOZ
        // ----------------------------------

        if (profesora != null)
        {
            profesora.Siguiente();
        }


        Debug.Log(
            $"[CORRECTO] Seña detectada: {senaDetectada}"
        );


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
            // ----------------------------------
            // FIN DEL NIVEL
            // ----------------------------------

            if (textoProfesora != null)
            {
                textoProfesora.text =
                    "¡Felicidades! Has completado todas las señas.";
            }

            if (textoFeedback != null)
            {
                textoFeedback.text =
                    "<color=yellow>¡Nivel Completado!</color>";
            }

            if (profesora != null)
            {
                profesora.Finalizar();
            }

            Debug.Log("[JUEGO] ¡Todas las señas completadas!");

            // Esperar antes de cambiar de escena
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