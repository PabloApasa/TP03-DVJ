using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Unity;

public class LectorManoMediaPipe : MonoBehaviour
{
    [Header("Referencias del Juego")]
    public GameManager gameManager;

    [Header("Componente de MediaPipe")]
    public HandLandmarkerResultAnnotationController controllerAnotaciones;

    private Vector2[] puntosNormalizados = new Vector2[21];

    void Update()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        if (controllerAnotaciones == null)
            controllerAnotaciones = FindObjectOfType<HandLandmarkerResultAnnotationController>();

        if (controllerAnotaciones != null && gameManager != null)
        {
            ProcesarDatosMano();
        }
    }

    private void ProcesarDatosMano()
    {
        if (controllerAnotaciones == null || controllerAnotaciones.transform.childCount == 0)
            return;

        // 1. Buscamos en toda la jerarquía de la mano el transform que contenga los 21 puntos
        Transform contenedorPuntos = BuscarContenedorDeLandmarks(controllerAnotaciones.transform);

        if (contenedorPuntos == null)
        {
            // Esto ayuda a depurar cuántos objetos intermedios existen si la mano está detectada pero los puntos no se han instanciado
            Debug.Log("[DIAGNÓSTICO] Esperando que se instancien los 21 puntos en la jerarquía...");
            return;
        }

        // 2. Extracción de posiciones
        Vector3 posicionMuneca = contenedorPuntos.GetChild(0).position;
        Vector3 posicionNudilloMedio = contenedorPuntos.GetChild(9).position;

        float tamanoMano = Vector3.Distance(posicionMuneca, posicionNudilloMedio);
        if (tamanoMano < 0.001f) tamanoMano = 1f;

        for (int i = 0; i < 21; i++)
        {
            Vector3 puntoActual = contenedorPuntos.GetChild(i).position;
            Vector3 puntoRelativo = puntoActual - posicionMuneca;

            puntosNormalizados[i] = new Vector2(
                puntoRelativo.x / tamanoMano,
                -puntoRelativo.y / tamanoMano
            );
        }

        // 3. Enviar al GameManager
        Debug.Log("¡[ÉXITO TOTAL] 21 Puntos leídos correctamente! Evaluando seña...");
        if (gameManager != null)
        {
            gameManager.ProcesarLandmarksMediaPipe(puntosNormalizados);
        }
    }

    // Método auxiliar para encontrar el objeto real que guarda las 21 articulaciones
    private Transform BuscarContenedorDeLandmarks(Transform padre)
    {
        if (padre.childCount >= 21)
            return padre;

        foreach (Transform hijo in padre)
        {
            Transform resultado = BuscarContenedorDeLandmarks(hijo);
            if (resultado != null)
                return resultado;
        }

        return null;
    }
}