using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // <--- 1. Agregamos la librería de TextMeshPro

[System.Serializable]
public struct SenaItem
{
    public string idSena;       // "ILOVEYOU", "NO", "FAMILIA"
    public string nombreAMostrar;
    public Sprite imagenSena;
}

public class GameManager : MonoBehaviour
{
<<<<<<< Updated upstream
=======
    [System.Serializable]
    public struct ConfigSena
    {
        public string nombreSena;
        public Sprite imagenSena;
    }

    [Header("Configuración de Señas")]
    public List<ConfigSena> listaSenas;
    private int indiceSenaActual = 0;

>>>>>>> Stashed changes
    [Header("Referencias de UI")]
    public TMP_Text textoProfesora; // <--- 2. Cambiamos 'Text' por 'TMP_Text'
    public Image imagenPizarron;
    public TMP_Text textoFeedback;  // <--- 3. Cambiamos 'Text' por 'TMP_Text'

<<<<<<< Updated upstream
    [Header("Lista de Señas a Enseñar")]
    public List<SenaItem> listaSenas;

    private int indiceActual = 0;
    private bool esperandoEntrada = false;
=======
    [Header("Profesora")]
    public ProfesoraTTS profesora;

    [Header("Estado del Juego")]
    [HideInInspector] public string senaActualTarget = "";

    private bool esperandoSena = true;
>>>>>>> Stashed changes

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
        if (textoFeedback != null) textoFeedback.text = "";
        CargarSenaActual();
    }


    void Update()
    {
<<<<<<< Updated upstream
        // SIMULACIÓN TECLADO:
        // Presiona 1 para "I LOVE YOU", 2 para "NO", 3 para "FAMILIA"
        if (esperandoEntrada)
=======
        // ----------------------------------
        // TECLAS DE PRUEBA
        // ----------------------------------

        if (Input.GetKeyDown(KeyCode.Alpha1) && esperandoSena)
>>>>>>> Stashed changes
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) OnSenaDetectada("ILOVEYOU");
            if (Input.GetKeyDown(KeyCode.Alpha2)) OnSenaDetectada("NO");
            if (Input.GetKeyDown(KeyCode.Alpha3)) OnSenaDetectada("FAMILIA");
        }
    }

<<<<<<< Updated upstream
    void CargarSenaActual()
    {
        if (indiceActual < listaSenas.Count)
        {
            SenaItem sena = listaSenas[indiceActual];
            textoProfesora.text = "¡Hola! Hoy aprenderemos la seña: " + sena.nombreAMostrar;
            imagenPizarron.sprite = sena.imagenSena;
            esperandoEntrada = true;
        }
        else
        {
            textoProfesora.text = "¡Excelente trabajo! Has completado las 3 señas.";
            imagenPizarron.gameObject.SetActive(false);
            textoFeedback.text = "¡JUEGO COMPLETADO!";
=======

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
>>>>>>> Stashed changes
        }
    }

    public void OnSenaDetectada(string idSena)
    {
        if (!esperandoEntrada) return;

        if (idSena == listaSenas[indiceActual].idSena)
        {
            StartCoroutine(RutinaSenaCorrecta());
        }
    }

    public void ProcesarLandmarksMediaPipe(Vector2[] puntosMano)
    {
        if (!esperandoEntrada) return;

        string senaActualTarget = listaSenas[indiceActual].idSena;

        if (senaActualTarget == "ILOVEYOU" && EvaluadorSenas.EsILoveYou(puntosMano))
        {
            OnSenaDetectada("ILOVEYOU");
        }
        else if (senaActualTarget == "NO" && EvaluadorSenas.EsSenaNo(puntosMano))
        {
            OnSenaDetectada("NO");
        }
        else if (senaActualTarget == "FAMILIA" && EvaluadorSenas.EsFamilia(puntosMano))
        {
            OnSenaDetectada("FAMILIA");
        }
    }

    IEnumerator RutinaSenaCorrecta()
    {
        esperandoEntrada = false;
        textoFeedback.text = "¡CORRECTO!";
        yield return new WaitForSeconds(2.0f);
        textoFeedback.text = "";

        indiceActual++;
        CargarSenaActual();
    }
}