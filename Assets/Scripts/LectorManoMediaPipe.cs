using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample.HandLandmarkDetection;

public class LectorManoMediaPipe : MonoBehaviour
{
    [Header("Referencias del Juego")]
    public GameManager gameManager;

    [Header("Componentes de MediaPipe")]
    public HandLandmarkerResultAnnotationController controllerAnotaciones;
    public HandLandmarkerRunner handLandmarkerRunner;

    private Vector2[] puntosNormalizados = new Vector2[21];


    void Update()
    {
        // Buscar GameManager
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

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
    }
}a