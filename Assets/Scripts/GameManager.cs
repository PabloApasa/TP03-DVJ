using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public struct ConfigSena
    {
        public string nombreSena; // Ej: "ILOVEYOU", "NO", "FAMILIA"
        public Sprite imagenSena; // Imagen de la seña que se muestra en el pizarrón
    }

    [Header("Configuración de Señas")]
    public List<ConfigSena> listaSenas;
    private int indiceSenaActual = 0;

    [Header("Referencias de UI")]
    public TMP_Text textoProfesora;
    public TMP_Text textoFeedback;
    public UnityEngine.UI.Image imagenDisplaySena;

    [Header("Estado del Juego")]
    [HideInInspector] public string senaActualTarget = "";
    private bool esperandoSena = true;

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
        // Teclas de prueba rápida (1, 2, 3) para probar la UI sin cámara
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

    // Este es el método que llama LectorManoMediaPipe.cs en cada frame
    public void ProcesarLandmarksMediaPipe(Vector2[] puntos)
    {
        if (!esperandoSena || puntos == null || puntos.Length < 21) return;

        // --- MONITOREO DE DISTANCIAS ---
        float dIndice = puntos[8].magnitude;
        float dMedio = puntos[12].magnitude;
        float dMenique = puntos[20].magnitude;

        Debug.Log($"[DATOS MANO] Índice: {dIndice:F2} | Medio: {dMedio:F2} | Meñique: {dMenique:F2}");

        // Evaluaciones
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

    private void CargarSenaActual()
    {
        esperandoSena = true;
        ConfigSena actual = listaSenas[indiceSenaActual];
        senaActualTarget = actual.nombreSena;

        if (textoProfesora != null)
            textoProfesora.text = $"Haz la seña: <b>{senaActualTarget}</b>";

        if (textoFeedback != null)
            textoFeedback.text = "";

        if (imagenDisplaySena != null && actual.imagenSena != null)
            imagenDisplaySena.sprite = actual.imagenSena;
    }

    private void OnSenaDetectada(string senaDetectada)
    {
        esperandoSena = false;

        if (textoFeedback != null)
            textoFeedback.text = "<color=green>¡CORRECTO!</color>";

        StartCoroutine(RutinaSiguienteSena());
    }

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
            // Fin del nivel o juego
            if (textoProfesora != null)
                textoProfesora.text = "¡Felicidades! Has completado todas las señas.";

            if (textoFeedback != null)
                textoFeedback.text = "<color=yellow>¡Nivel Completado!</color>";
        }
    }
}