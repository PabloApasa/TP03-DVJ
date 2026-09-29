using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [Header("Estado del Juego")]
    [HideInInspector] public string senaActualTarget = "";

    private bool esperandoSena = true;

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
        if (listaSenas != null && listaSenas.Count > 0)
        {
            CargarSenaActual();
        }
        else
        {
            Debug.LogError("¡Atención! La lista de señas está vacía en el Inspector.");
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
        if (!esperandoSena)
            return;

        if (puntos == null || puntos.Length < 21)
            return;


        // ----------------------------------
        // TIEMPO DE PREPARACIÓN
        // ----------------------------------

        if (Time.time - tiempoInicioSena < tiempoPreparacion)
        {
            framesCorrectos = 0;
            return;
        }


        // ----------------------------------
        // MONITOREO
        // ----------------------------------

        float dIndice = puntos[8].magnitude;
        float dMedio = puntos[12].magnitude;
        float dMenique = puntos[20].magnitude;

        Debug.Log(
            $"[DATOS MANO] Índice: {dIndice:F2} | " +
            $"Medio: {dMedio:F2} | " +
            $"Meñique: {dMenique:F2}"
        );


        // ----------------------------------
        // EVALUAR SEÑA ACTUAL
        // ----------------------------------

        bool senaCorrecta = false;

        if (senaActualTarget == "ILOVEYOU")
        {
            senaCorrecta = EvaluadorSenas.EsILoveYou(puntos);
        }
        else if (senaActualTarget == "NO")
        {
            senaCorrecta = EvaluadorSenas.EsSenaNo(puntos);
        }
        else if (senaActualTarget == "FAMILIA")
        {
            senaCorrecta = EvaluadorSenas.EsFamilia(puntos);
        }


        // ----------------------------------
        // CONTROL DE FRAMES
        // ----------------------------------

        if (senaCorrecta)
        {
            framesCorrectos++;

            Debug.Log(
                $"[SEÑA] {senaActualTarget} correcta. " +
                $"Frames: {framesCorrectos}/{framesNecesarios}"
            );


            if (framesCorrectos >= framesNecesarios)
            {
                OnSenaDetectada(senaActualTarget);
            }
        }
        else
        {
            // Si pierde la posición correcta,
            // reiniciamos el contador.

            framesCorrectos = 0;
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
        }
    }
}