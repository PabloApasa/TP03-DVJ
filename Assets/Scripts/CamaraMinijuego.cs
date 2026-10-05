//using UnityEngine;

//public class CamaraMinijuego : MonoBehaviour
//{
//    [Header("Jugador")]
//    public Transform jugador;

//    [Header("Configuración de Seguimiento")]
//    public float suavidad = 5f;
//    public float offsetY = 0f; // Margen vertical respecto al jugador

//    [Header("Límites del Escenario (Bounds)")]
//    public bool usarLimites = true;

//    [Tooltip("Límite horizontal mínimo (Lado Izquierdo) y máximo (Lado Derecho)")]
//    public float minX = -10f;
//    public float maxX = 10f;

//    [Tooltip("Límite vertical mínimo (Suelo) y máximo (Techo)")]
//    public float minY = 0f;
//    public float maxY = 20f;

//    private float posicionZ;

//    private void Start()
//    {
//        // Guardamos la profundidad Z inicial de la cámara
//        posicionZ = transform.position.z;
//    }

//    private void LateUpdate()
//    {
//        if (jugador == null)
//            return;

//        // 1. Obtenemos la posición objetivo basada en el jugador
//        float objetivoX = jugador.position.x;
//        float objetivoY = jugador.position.y + offsetY;

//        // 2. Aplicamos los límites de movimiento si la opción está activa
//        if (usarLimites)
//        {
//            objetivoX = Mathf.Clamp(objetivoX, minX, maxX);
//            objetivoY = Mathf.Clamp(objetivoY, minY, maxY);
//        }

//        Vector3 nuevaPosicion = new Vector3(objetivoX, objetivoY, posicionZ);

//        // 3. Interpolación suave hacia la posición limitada
//        transform.position = Vector3.Lerp(
//            transform.position,
//            nuevaPosicion,
//            suavidad * Time.deltaTime
//        );
//    }

//    // Dibujar los límites en la vista de Scene para configurarlos visualmente
//    private void OnDrawGizmosSelected()
//    {
//        if (!usarLimites) return;

//        Gizmos.color = Color.cyan;
//        Vector3 centro = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0f);
//        Vector3 tamano = new Vector3(maxX - minX, maxY - minY, 1f);
//        Gizmos.DrawWireCube(centro, tamano);
//    }
//}

using UnityEngine;

public class CamaraMinijuego : MonoBehaviour
{
    [Header("Jugador")]
    public JugadorMinijuego jugador;

    [Header("Configuración de Seguimiento")]
    public float suavidad = 5f;
    public float offsetY = 0f; // Margen vertical respecto al jugador

    [Header("Límites del Escenario (Bounds)")]
    public bool usarLimites = true;

    [Tooltip("Límite horizontal mínimo (Lado Izquierdo) y máximo (Lado Derecho)")]
    public float minX = -10f;
    public float maxX = 10f;

    [Tooltip("Límite vertical mínimo (Suelo) y máximo (Techo)")]
    public float minY = 0f;
    public float maxY = 20f;

    private float posicionZ;

    private void Start()
    {
        posicionZ = transform.position.z;

        if (jugador == null)
        {
            jugador = FindObjectOfType<JugadorMinijuego>();
        }
    }

    private void LateUpdate()
    {
        if (jugador == null || jugador.juegoTerminado)
            return;

        float objetivoX = jugador.transform.position.x;
        float objetivoY = jugador.transform.position.y + offsetY;

        if (usarLimites)
        {
            objetivoX = Mathf.Clamp(objetivoX, minX, maxX);
            objetivoY = Mathf.Clamp(objetivoY, minY, maxY);
        }

        Vector3 nuevaPosicion = new Vector3(objetivoX, objetivoY, posicionZ);

        transform.position = Vector3.Lerp(
            transform.position,
            nuevaPosicion,
            suavidad * Time.deltaTime
        );
    }

    // Método para teletransportar la cámara al instante al respawnear
    public void ResetearPosicion()
    {
        if (jugador == null) return;

        float objetivoX = jugador.transform.position.x;
        float objetivoY = jugador.transform.position.y + offsetY;

        if (usarLimites)
        {
            objetivoX = Mathf.Clamp(objetivoX, minX, maxX);
            objetivoY = Mathf.Clamp(objetivoY, minY, maxY);
        }

        transform.position = new Vector3(objetivoX, objetivoY, posicionZ);
    }

    private void OnDrawGizmosSelected()
    {
        if (!usarLimites) return;

        Gizmos.color = Color.cyan;
        Vector3 centro = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0f);
        Vector3 tamano = new Vector3(maxX - minX, maxY - minY, 1f);
        Gizmos.DrawWireCube(centro, tamano);
    }
}