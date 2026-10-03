using UnityEngine;

public class JugadorMinijuego : MonoBehaviour
{
    [Header("Movimiento Horizontal")]
    public Rigidbody2D rb;
    public float velocidadHorizontal = 3f;

    [Header("Animator")]
    public Animator animator;

    [Header("Fuerza de Salto")]
    public float fuerzaSalto = 5f;  // Salto Corto / Normal
    public float fuerzaSalto2 = 8f;  // Salto Medio
    public float fuerzaSalto3 = 10f; // Salto Alto

    [Header("Detección de Suelo")]
    public Transform puntoSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    private bool estaEnSuelo;

    // Control para evitar saltos infinitos continuos mientras se mantiene la seña
    private bool senaProcesada = false;

    private void Update()
    {
        // 1. Obtener los puntos de MediaPipe (Reemplaza este método por tu proveedor real de puntos)
        Vector2[] puntosMano = ObtenerPuntosMediaPipe();

        // Si no hay datos válidos de la mano, reiniciamos el estado y salimos
        if (puntosMano == null || puntosMano.Length < 21)
        {
            senaProcesada = false;
            return;
        }

        // 2. Evaluamos las señas usando el EvaluadorSenas estático
        bool esNo = EvaluadorSenas.EsSenaNo(puntosMano);
        bool esILoveYou = EvaluadorSenas.EsILoveYou(puntosMano);
        bool esFamilia = EvaluadorSenas.EsFamilia(puntosMano);

        // 3. Si el jugador está haciendo alguna seña y aún no ha sido procesada para este gesto
        if ((esNo || esILoveYou || esFamilia) && !senaProcesada)
        {
            if (esNo)
            {
                Saltar();   // Fuerza 1 (Ej. Salto Normal)
            }
            else if (esILoveYou)
            {
                Saltar2();  // Fuerza 2 (Ej. Salto Medio)
            }
            else if (esFamilia)
            {
                Saltar3();  // Fuerza 3 (Ej. Salto Alto)
            }

            senaProcesada = true; // Bloquea nuevos saltos hasta que relaje la mano o cambie de seña
        }
        // Si ya no está haciendo ninguna seña, permitimos volver a detectar una nueva
        else if (!esNo && !esILoveYou && !esFamilia)
        {
            senaProcesada = false;
        }
    }

    private void FixedUpdate()
    {
        if (rb == null)
            return;

        // Movimiento horizontal automático
        rb.linearVelocity = new Vector2(
            velocidadHorizontal,
            rb.linearVelocity.y
        );

        // Comprobar si está tocando el suelo
        estaEnSuelo = Physics2D.OverlapCircle(
            puntoSuelo.position,
            radioSuelo,
            capaSuelo
        );

        // Actualizar animación
        ActualizarAnimacion();
    }

    private void ActualizarAnimacion()
    {
        if (animator == null)
            return;

        // Si está en el aire, activa la animación de salto
        animator.SetBool("Saltando", !estaEnSuelo);
    }

    // --- Métodos de Salto ---

    public void Saltar()
    {
        EjecutarSalto(fuerzaSalto, "Saltó (Seña 'NO')");
    }

    public void Saltar2()
    {
        EjecutarSalto(fuerzaSalto2, "Saltó 2 (Seña 'I LOVE YOU')");
    }

    public void Saltar3()
    {
        EjecutarSalto(fuerzaSalto3, "Saltó 3 (Seña 'FAMILIA')");
    }

    private void EjecutarSalto(float fuerza, string logMensaje)
    {
        if (rb == null) return;

        if (!estaEnSuelo)
        {
            Debug.Log("No puede saltar porque está en el aire.");
            return;
        }

        // Reseteamos la velocidad vertical antes de aplicar el impulso
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        rb.AddForce(
            Vector2.up * fuerza,
            ForceMode2D.Impulse
        );

        Debug.Log("¡" + logMensaje + "!");
    }

    // Método de marcador de posición para obtener los puntos de MediaPipe
    private Vector2[] ObtenerPuntosMediaPipe()
    {
        // AQUÍ CONECTAS TU FUENTE REAL DE MEDIAPIPE EN UNITY
        // Ejemplo: return MiMediaPipeManager.Instance.ObtenerLandmarksMano();
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoSuelo == null)
            return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            puntoSuelo.position,
            radioSuelo
        );
    }
}   