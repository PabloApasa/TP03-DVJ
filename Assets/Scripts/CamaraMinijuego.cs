
using UnityEngine;

public class CamaraMinijuego : MonoBehaviour
{
    [Header("Jugador")]
    public Transform jugador;

    [Header("Configuración")]
    public float suavidad = 5f;

    private float posicionY;
    private float posicionZ;

    private void Start()
    {
        // Guardamos la altura inicial de la cámara
        posicionY = transform.position.y;
        posicionZ = transform.position.z;
    }

    private void LateUpdate()
    {
        if (jugador == null)
            return;

        // La cámara sigue solamente la posición X del jugador
        Vector3 nuevaPosicion = new Vector3(
            jugador.position.x,
            posicionY,
            posicionZ
        );

        // Movimiento suave de la cámara
        transform.position = Vector3.Lerp(
            transform.position,
            nuevaPosicion,
            suavidad * Time.deltaTime
        );
    }
}