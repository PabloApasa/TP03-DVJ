using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample.HandLandmarkDetection;

public class LectorManoMediaPipe : MonoBehaviour
{
    [Header("Referencias del Juego")]
    public GameManager gameManager;

<<<<<<< Updated upstream
    // Referencia al componente de la escena oficial que dibuja los puntos de la mano
    [Header("Componente de MediaPipe")]
    public MultiHandLandmarkListAnnotationController controllerAnotaciones;
=======
    [Header("Componentes de MediaPipe")]
    public HandLandmarkerResultAnnotationController controllerAnotaciones;
    public HandLandmarkerRunner handLandmarkerRunner;
>>>>>>> Stashed changes

    private Vector2[] puntosNormalizados = new Vector2[21];


    void Update()
    {
<<<<<<< Updated upstream
        // Buscar automáticamente el GameManager si no está asignado
=======
        // Buscar GameManager
>>>>>>> Stashed changes
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

<<<<<<< Updated upstream
        // Buscar automáticamente el controlador de anotaciones en la escena si no está asignado
        if (controllerAnotaciones == null)
        {
            controllerAnotaciones = FindObjectOfType<MultiHandLandmarkListAnnotationController>();
        }

        // Si encontramos la detección activa en pantalla, extraemos las coordenadas
        if (controllerAnotaciones != null && gameManager != null)
=======
        // Buscar controlador de landmarks
        if (controllerAnotaciones == null)
            controllerAnotaciones =
                FindObjectOfType<HandLandmarkerResultAnnotationController>();

        // Buscar Runner de MediaPipe
        if (handLandmarkerRunner == null)
            handLandmarkerRunner =
                FindObjectOfType<HandLandmarkerRunner>();


        // Si falta alguna referencia, esperamos
        if (controllerAnotaciones == null ||
            gameManager == null ||
            handLandmarkerRunner == null)
>>>>>>> Stashed changes
        {
            return;
        }


        // =====================================================
        // COMPROBAR SI REALMENTE HAY UNA MANO
        // =====================================================

        if (!handLandmarkerRunner.HayManoDetectada)
        {
            // NO procesamos los landmarks antiguos
            return;
        }


        // =====================================================
        // HAY UNA MANO DETECTADA
        // =====================================================

        ProcesarDatosMano();
    }


    private void ProcesarDatosMano()
    {
<<<<<<< Updated upstream
        if (controllerAnotaciones.transform.childCount > 0)
        {
            Transform primeraMano = controllerAnotaciones.transform.GetChild(0);

            if (primeraMano.childCount >= 21)
            {
                // 1. Tomamos la posición local de la muñeca (punto 0)
                Vector2 posicionMuneca = primeraMano.GetChild(0).localPosition;

                // 2. Tomamos la posición del nudillo medio (punto 9) para calcular la escala
                Vector2 posicionNudilloMedio = primeraMano.GetChild(9).localPosition;
                float tamanoMano = Vector2.Distance(posicionMuneca, posicionNudilloMedio);

                if (tamanoMano < 0.001f) tamanoMano = 1f;

                // 3. Normalizamos e invertimos el eje Y
                for (int i = 0; i < 21; i++)
                {
                    Vector2 puntoActual = primeraMano.GetChild(i).localPosition;
                    Vector2 puntoRelativo = puntoActual - posicionMuneca;

                    // Dividimos por el tamaño e invertimos Y para alinear con la lógica del Evaluador
                    puntosNormalizados[i] = new Vector2(
                        puntoRelativo.x / tamanoMano,
                        -puntoRelativo.y / tamanoMano
                    );
                }

                // 4. Enviamos los puntos corregidos al GameManager
                gameManager.ProcesarLandmarksMediaPipe(puntosNormalizados);
            }
        }
=======
        if (controllerAnotaciones == null ||
            controllerAnotaciones.transform.childCount == 0)
        {
            return;
        }


        // Buscar el contenedor que tiene los 21 puntos
        Transform contenedorPuntos =
            BuscarContenedorDeLandmarks(
                controllerAnotaciones.transform
            );


        if (contenedorPuntos == null)
        {
            Debug.Log(
                "[DIAGNÓSTICO] Esperando que se instancien " +
                "los 21 puntos en la jerarquía..."
            );

            return;
        }


        // =====================================================
        // EXTRAER POSICIONES
        // =====================================================

        Vector3 posicionMuneca =
            contenedorPuntos.GetChild(0).position;

        Vector3 posicionNudilloMedio =
            contenedorPuntos.GetChild(9).position;


        float tamanoMano =
            Vector3.Distance(
                posicionMuneca,
                posicionNudilloMedio
            );


        if (tamanoMano < 0.001f)
            tamanoMano = 1f;


        // =====================================================
        // NORMALIZAR LOS 21 LANDMARKS
        // =====================================================

        for (int i = 0; i < 21; i++)
        {
            Vector3 puntoActual =
                contenedorPuntos.GetChild(i).position;

            Vector3 puntoRelativo =
                puntoActual - posicionMuneca;


            puntosNormalizados[i] = new Vector2(
                puntoRelativo.x / tamanoMano,
                -puntoRelativo.y / tamanoMano
            );
        }


        // =====================================================
        // ENVIAR AL GAME MANAGER
        // =====================================================

        gameManager.ProcesarLandmarksMediaPipe(
            puntosNormalizados
        );
    }


    // =========================================================
    // BUSCAR CONTENEDOR DE LOS 21 LANDMARKS
    // =========================================================

    private Transform BuscarContenedorDeLandmarks(
        Transform padre)
    {
        if (padre.childCount >= 21)
            return padre;


        foreach (Transform hijo in padre)
        {
            Transform resultado =
                BuscarContenedorDeLandmarks(hijo);


            if (resultado != null)
                return resultado;
        }


        return null;
>>>>>>> Stashed changes
    }
}