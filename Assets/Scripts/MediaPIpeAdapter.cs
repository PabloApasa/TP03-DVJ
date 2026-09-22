using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Unity.Sample.HandLandmarkDetection;

public class MediaPipeAdapter : MonoBehaviour
{
    [Header("Referencias")]
    public GameManager gameManager;
    public HandLandmarkerRunner handLandmarkerRunner; 
    // Arreglo donde guardamos los 21 puntos procesados de la mano
    private Vector2[] puntosNormalizados = new Vector2[21];

    private void OnEnable()
    {
        if (handLandmarkerRunner == null)
        {
            handLandmarkerRunner = FindObjectOfType<HandLandmarkerRunner>();
        }

        if (handLandmarkerRunner != null)
        {
            // Cambiar el nombre del evento al correcto según la convención encontrada en los tipos:
            // El evento correcto parece ser OnHandLandmarkResult (sin "er" al final de "Landmark").
            handLandmarkerRunner.OnHandLandmarkResult += OnHandLandmarksReceived;
        }
    }

    private void OnDisable()
    {
        if (handLandmarkerRunner != null)
        {
            handLandmarkerRunner.OnHandLandmarkResult -= OnHandLandmarksReceived;
        }
    }

    public void OnHandLandmarksReceived(HandLandmarkerResult result)
    {
        //verificar si detecto una mano al menos
        if (result.handLandmarks == null || result.handLandmarks.Count == 0) return;
        // tomamos la primer mano detectada
        var landmarks = result.handLandmarks[0].landmarks;
        if (landmarks == null || landmarks.Count < 21) return;
        // observamos posicion de la muneca
        var writs = landmarks[0];
        Vector2 posicionMuneca = new Vector2(writs.x, writs.y);
        // normalizamos los 21 puntos restando el de la muneca
        for (int i = 0; i < 21; i++)
        {
            var lm = landmarks[i];
            Vector2 puntoActual = new Vector2(lm.x, lm.y);
            puntosNormalizados[i] = puntoActual - posicionMuneca;
        }
        //enviamos los puntos al game manager
        if (gameManager != null)
        {
            gameManager.ProcesarLandmarksMediaPipe(puntosNormalizados);
        }    
    }
}
