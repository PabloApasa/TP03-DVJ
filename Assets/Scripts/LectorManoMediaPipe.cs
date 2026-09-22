using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Unity;

public class LectorManoMediaPipe : MonoBehaviour
{
    [Header("Referencias del Juego")]
    public GameManager gameManager;

    // Referencia al componente de la escena oficial que dibuja los puntos de la mano
    [Header("Componente de MediaPipe")]
    public MultiHandLandmarkListAnnotationController controllerAnotaciones;

    private Vector2[] puntosNormalizados = new Vector2[21];

    void Update()
    {
        // Buscar automáticamente el GameManager si no está asignado
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        // Buscar automáticamente el controlador de anotaciones en la escena si no está asignado
        if (controllerAnotaciones == null)
        {
            controllerAnotaciones = FindObjectOfType<MultiHandLandmarkListAnnotationController>();
        }

        // Si encontramos la detección activa en pantalla, extraemos las coordenadas
        if (controllerAnotaciones != null && gameManager != null)
        {
            ProcesarDatosMano();
        }
    }

    private void ProcesarDatosMano()
    {
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
    }
}