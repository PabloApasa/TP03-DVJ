using UnityEngine;
using TMPro; // Si usas TextMeshPro

public class ContadorTiempo : MonoBehaviour
{
    [Header("Configuración del Tiempo")]
    [Tooltip("Tiempo en segundos (3 minutos = 180 segundos)")]
    public float tiempoRestante = 180f; // 3 minutos
    public bool contadorActivo = true;

    [Header("Referencias UI")]
    public TextMeshProUGUI textoTiempo; // Cambia a "public Text textoTiempo;" si usas UI Text normal
    public GameObject panelGameOver;   // Tu panel de Game Over o Modal Panel

    void Update()
    {
        if (contadorActivo)
        {
            if (tiempoRestante > 0)
            {
                tiempoRestante -= Time.deltaTime; // Resta el tiempo paso a paso
                ActualizarTextoTiempo(tiempoRestante);
            }
            else
            {
                // El tiempo se terminó
                tiempoRestante = 0;
                contadorActivo = false;
                ActualizarTextoTiempo(0);
                EjecutarGameOver();
            }
        }
    }

    void ActualizarTextoTiempo(float tiempoEnSegundos)
    {
        // Convierte los segundos a formato Minutos:Segundos (03:00)
        int minutos = Mathf.FloorToInt(tiempoEnSegundos / 60);
        int segundos = Mathf.FloorToInt(tiempoEnSegundos % 60);

        textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    void EjecutarGameOver()
    {
        Debug.Log("¡Tiempo agotado! Fin del juego.");

        // Activa la pantalla / panel de Game Over
        if (panelGameOver != null)
        {
            panelGameOver.SetActive(true);
        }

        // Detener el tiempo de física del juego (opcional)
        Time.timeScale = 0f;
    }
}
