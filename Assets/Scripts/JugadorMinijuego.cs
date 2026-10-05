//using UnityEngine;

//public class JugadorMinijuego : MonoBehaviour
//{
//    [Header("Movimiento Horizontal")]
//    public Rigidbody2D rb;
//    public float velocidadHorizontal = 3f;

//    [Header("Animator")]
//    public Animator animator;

//    [Header("Fuerza de Salto")]
//    public float fuerzaSalto = 5f;  // Salto Corto / Normal
//    public float fuerzaSalto2 = 8f;  // Salto Medio
//    public float fuerzaSalto3 = 10f; // Salto Alto

//    [Header("Detección de Suelo")]
//    public Transform puntoSuelo;
//    public float radioSuelo = 0.2f;
//    public LayerMask capaSuelo;

//    private bool estaEnSuelo;

//    // Control para evitar saltos infinitos continuos mientras se mantiene la seña
//    private bool senaProcesada = false;

//    private void Update()
//    {
//        // 1. Obtener los puntos de MediaPipe (Reemplaza este método por tu proveedor real de puntos)
//        Vector2[] puntosMano = ObtenerPuntosMediaPipe();

//        // Si no hay datos válidos de la mano, reiniciamos el estado y salimos
//        if (puntosMano == null || puntosMano.Length < 21)
//        {
//            senaProcesada = false;
//            return;
//        }

//        // 2. Evaluamos las señas usando el EvaluadorSenas estático
//        bool esNo = EvaluadorSenas.EsSenaNo(puntosMano);
//        bool esILoveYou = EvaluadorSenas.EsILoveYou(puntosMano);
//        bool esFamilia = EvaluadorSenas.EsFamilia(puntosMano);

//        // 3. Si el jugador está haciendo alguna seña y aún no ha sido procesada para este gesto
//        if ((esNo || esILoveYou || esFamilia) && !senaProcesada)
//        {
//            if (esNo)
//            {
//                Saltar();   // Fuerza 1 (Ej. Salto Normal)
//            }
//            else if (esILoveYou)
//            {
//                Saltar2();  // Fuerza 2 (Ej. Salto Medio)
//            }
//            else if (esFamilia)
//            {
//                Saltar3();  // Fuerza 3 (Ej. Salto Alto)
//            }

//            senaProcesada = true; // Bloquea nuevos saltos hasta que relaje la mano o cambie de seña
//        }
//        // Si ya no está haciendo ninguna seña, permitimos volver a detectar una nueva
//        else if (!esNo && !esILoveYou && !esFamilia)
//        {
//            senaProcesada = false;
//        }
//    }

//    private void FixedUpdate()
//    {
//        if (rb == null)
//            return;

//        // Movimiento horizontal automático
//        rb.linearVelocity = new Vector2(
//            velocidadHorizontal,
//            rb.linearVelocity.y
//        );

//        // Comprobar si está tocando el suelo
//        estaEnSuelo = Physics2D.OverlapCircle(
//            puntoSuelo.position,
//            radioSuelo,
//            capaSuelo
//        );

//        // Actualizar animación
//        ActualizarAnimacion();
//    }

//    private void ActualizarAnimacion()
//    {
//        if (animator == null)
//            return;

//        // Si está en el aire, activa la animación de salto
//        animator.SetBool("Saltando", !estaEnSuelo);
//    }

//    // --- Métodos de Salto ---

//    public void Saltar()
//    {
//        EjecutarSalto(fuerzaSalto, "Saltó (Seña 'NO')");
//    }

//    public void Saltar2()
//    {
//        EjecutarSalto(fuerzaSalto2, "Saltó 2 (Seña 'I LOVE YOU')");
//    }

//    public void Saltar3()
//    {
//        EjecutarSalto(fuerzaSalto3, "Saltó 3 (Seña 'FAMILIA')");
//    }

//    private void EjecutarSalto(float fuerza, string logMensaje)
//    {
//        if (rb == null) return;

//        if (!estaEnSuelo)
//        {
//            Debug.Log("No puede saltar porque está en el aire.");
//            return;
//        }

//        // Reseteamos la velocidad vertical antes de aplicar el impulso
//        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

//        rb.AddForce(
//            Vector2.up * fuerza,
//            ForceMode2D.Impulse
//        );

//        Debug.Log("¡" + logMensaje + "!");
//    }

//    // Método de marcador de posición para obtener los puntos de MediaPipe
//    private Vector2[] ObtenerPuntosMediaPipe()
//    {
//        // AQUÍ CONECTAS TU FUENTE REAL DE MEDIAPIPE EN UNITY
//        // Ejemplo: return MiMediaPipeManager.Instance.ObtenerLandmarksMano();
//        return null;
//    }

//    private void OnDrawGizmosSelected()
//    {
//        if (puntoSuelo == null)
//            return;

//        Gizmos.color = Color.green;

//        Gizmos.DrawWireSphere(
//            puntoSuelo.position,
//            radioSuelo
//        );
//    }
//}   

//using UnityEngine;

//public class JugadorMinijuego : MonoBehaviour
//{
//    [Header("Movimiento Horizontal")]
//    public Rigidbody2D rb;
//    public float velocidadHorizontal = 3f;

//    [Header("Animator")]
//    public Animator animator;

//    [Header("Fuerza de Salto")]
//    public float fuerzaSalto = 5f;  // Salto Corto / Normal
//    public float fuerzaSalto2 = 8f;  // Salto Medio
//    public float fuerzaSalto3 = 10f; // Salto Alto

//    [Header("Detección de Suelo")]
//    public Transform puntoSuelo;
//    public float radioSuelo = 0.2f;
//    public LayerMask capaSuelo;

//    private bool estaEnSuelo;

//    private void FixedUpdate()
//    {
//        if (rb == null)
//            return;

//        // Movimiento horizontal automático
//        rb.linearVelocity = new Vector2(
//            velocidadHorizontal,
//            rb.linearVelocity.y
//        );

//        // Comprobar si está tocando el suelo
//        estaEnSuelo = Physics2D.OverlapCircle(
//            puntoSuelo.position,
//            radioSuelo,
//            capaSuelo
//        );

//        // Actualizar animación
//        ActualizarAnimacion();
//    }

//    private void ActualizarAnimacion()
//    {
//        if (animator == null)
//            return;

//        // Si está en el aire, activa la animación de salto
//        animator.SetBool("Saltando", !estaEnSuelo);
//    }

//    // --- Métodos de Salto ---

//    public void Saltar()
//    {
//        EjecutarSalto(fuerzaSalto, "Saltó (Seña 'NO')");
//    }

//    public void Saltar2()
//    {
//        EjecutarSalto(fuerzaSalto2, "Saltó 2 (Seña 'I LOVE YOU')");
//    }

//    public void Saltar3()
//    {
//        EjecutarSalto(fuerzaSalto3, "Saltó 3 (Seña 'FAMILIA')");
//    }

//    private void EjecutarSalto(float fuerza, string logMensaje)
//    {
//        if (rb == null) return;

//        if (!estaEnSuelo)
//        {
//            Debug.Log("No puede saltar porque está en el aire.");
//            return;
//        }

//        // Reseteamos la velocidad vertical antes de aplicar el impulso
//        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

//        rb.AddForce(
//            Vector2.up * fuerza,
//            ForceMode2D.Impulse
//        );

//        Debug.Log("¡" + logMensaje + "!");
//    }

//    private void OnDrawGizmosSelected()
//    {
//        if (puntoSuelo == null)
//            return;

//        Gizmos.color = Color.green;

//        Gizmos.DrawWireSphere(
//            puntoSuelo.position,
//            radioSuelo
//        );
//    }
//}


//using UnityEngine;

//public class JugadorMinijuego : MonoBehaviour
//{
//    [Header("Movimiento Horizontal")]
//    public Rigidbody2D rb;
//    public float velocidadHorizontal = 3f;

//    [Header("Animator")]
//    public Animator animator;

//    [Header("Fuerza de Salto")]
//    public float fuerzaSalto = 5f;  // Salto Corto
//    public float fuerzaSalto2 = 8f;  // Salto Medio
//    public float fuerzaSalto3 = 10f; // Salto Alto

//    [Header("Detección de Suelo")]
//    public Transform puntoSuelo;
//    public float radioSuelo = 0.2f;
//    public LayerMask capaSuelo;

//    [Header("Sistema de Vidas y Respawn")]
//    public int vidasMaximas = 3;
//    private int vidasActuales;
//    public Transform puntoRespawn;
//    public float limiteCaidaY = -10f; // Si cae por debajo de este Y, pierde 1 vida

//    private bool estaEnSuelo;
//    private bool juegoTerminado = false;

//    private void Start()
//    {
//        vidasActuales = vidasMaximas;
//        if (UIHUDMinijuego.Instancia != null)
//        {
//            UIHUDMinijuego.Instancia.ActualizarVidas(vidasActuales);
//        }
//    }

//    private void Update()
//    {
//        if (juegoTerminado) return;

//        // Verificar si se cayó del escenario
//        if (transform.position.y < limiteCaidaY)
//        {
//            CaerAlVacio();
//        }
//    }

//    private void FixedUpdate()
//    {
//        if (juegoTerminado || rb == null)
//            return;

//        // Movimiento horizontal automático
//        rb.linearVelocity = new Vector2(
//            velocidadHorizontal,
//            rb.linearVelocity.y
//        );

//        // Comprobar si está tocando el suelo
//        estaEnSuelo = Physics2D.OverlapCircle(
//            puntoSuelo.position,
//            radioSuelo,
//            capaSuelo
//        );

//        ActualizarAnimacion();
//    }

//    private void ActualizarAnimacion()
//    {
//        if (animator != null)
//        {
//            animator.SetBool("Saltando", !estaEnSuelo);
//        }
//    }

//    // --- Métodos de Salto ---

//    public void Saltar() => EjecutarSalto(fuerzaSalto, "Salto Corto");
//    public void Saltar2() => EjecutarSalto(fuerzaSalto2, "Salto Medio");
//    public void Saltar3() => EjecutarSalto(fuerzaSalto3, "Salto Alto");

//    private void EjecutarSalto(float fuerza, string logMensaje)
//    {
//        if (juegoTerminado || rb == null || !estaEnSuelo) return;

//        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
//        rb.AddForce(Vector2.up * fuerza, ForceMode2D.Impulse);
//    }

//    // --- Lógica de Daño, Caída y Meta ---

//    private void CaerAlVacio()
//    {
//        vidasActuales--;

//        if (UIHUDMinijuego.Instancia != null)
//        {
//            UIHUDMinijuego.Instancia.ActualizarVidas(vidasActuales);
//        }

//        if (vidasActuales <= 0)
//        {
//            GameOver();
//        }
//        else
//        {
//            Respawnear();
//        }
//    }

//    private void Respawnear()
//    {
//        if (rb != null)
//        {
//            rb.linearVelocity = Vector2.zero;
//        }

//        if (puntoRespawn != null)
//        {
//            transform.position = puntoRespawn.position;
//        }
//        else
//        {
//            transform.position = Vector3.zero;
//        }
//    }

//    private void GameOver()
//    {
//        juegoTerminado = true;
//        if (rb != null) rb.linearVelocity = Vector2.zero;

//        if (UIHUDMinijuego.Instancia != null)
//        {
//            UIHUDMinijuego.Instancia.MostrarGameOver();
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        // Detectar si el jugador tocó la meta final
//        if (collision.CompareTag("Meta") && !juegoTerminado)
//        {
//            juegoTerminado = true;
//            if (rb != null) rb.linearVelocity = Vector2.zero;

//            if (UIHUDMinijuego.Instancia != null)
//            {
//                UIHUDMinijuego.Instancia.MostrarVictoria();
//            }
//        }
//    }

//    private void OnDrawGizmosSelected()
//    {
//        if (puntoSuelo != null)
//        {
//            Gizmos.color = Color.green;
//            Gizmos.DrawWireSphere(puntoSuelo.position, radioSuelo);
//        }
//    }
//}

//using UnityEngine;

//public class JugadorMinijuego : MonoBehaviour
//{
//    [Header("Movimiento Horizontal")]
//    public Rigidbody2D rb;
//    public float velocidadHorizontal = 3f;

//    [Header("Animator")]
//    public Animator animator;

//    [Header("Fuerza de Salto")]
//    public float fuerzaSalto = 5f;  // Salto Corto
//    public float fuerzaSalto2 = 8f;  // Salto Medio
//    public float fuerzaSalto3 = 10f; // Salto Alto

//    [Header("Detección de Suelo")]
//    public Transform puntoSuelo;
//    public float radioSuelo = 0.2f;
//    public LayerMask capaSuelo;

//    [Header("Sistema de Vidas y Respawn")]
//    public int vidasMaximas = 3;
//    private int vidasActuales;
//    public Transform puntoRespawn;
//    public float limiteCaidaY = -10f;

//    private bool estaEnSuelo;
//    public bool juegoTerminado = false;
//    private bool procesandoCaida = false;

//    private void Start()
//    {
//        vidasActuales = vidasMaximas;
//        if (UIHUDMinijuego.Instancia != null)
//        {
//            UIHUDMinijuego.Instancia.ActualizarVidas(vidasActuales);
//        }
//    }

//    private void Update()
//    {
//        if (juegoTerminado) return;

//        if (transform.position.y < limiteCaidaY && !procesandoCaida)
//        {
//            CaerAlVacio();
//        }
//    }

//    private void FixedUpdate()
//    {
//        if (juegoTerminado || rb == null)
//            return;

//        rb.linearVelocity = new Vector2(
//            velocidadHorizontal,
//            rb.linearVelocity.y
//        );

//        estaEnSuelo = Physics2D.OverlapCircle(
//            puntoSuelo.position,
//            radioSuelo,
//            capaSuelo
//        );

//        ActualizarAnimacion();
//    }

//    private void ActualizarAnimacion()
//    {
//        if (animator != null)
//        {
//            animator.SetBool("Saltando", !estaEnSuelo);
//        }
//    }

//    // --- Métodos de Salto ---

//    public void Saltar() => EjecutarSalto(fuerzaSalto, "Salto Corto");
//    public void Saltar2() => EjecutarSalto(fuerzaSalto2, "Salto Medio");
//    public void Saltar3() => EjecutarSalto(fuerzaSalto3, "Salto Alto");

//    private void EjecutarSalto(float fuerza, string logMensaje)
//    {
//        if (juegoTerminado || rb == null || !estaEnSuelo) return;

//        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
//        rb.AddForce(Vector2.up * fuerza, ForceMode2D.Impulse);
//    }

//    // --- Lógica de Daño, Caída y Meta ---

//    private void CaerAlVacio()
//    {
//        procesandoCaida = true;
//        vidasActuales--;

//        if (UIHUDMinijuego.Instancia != null)
//        {
//            UIHUDMinijuego.Instancia.ActualizarVidas(vidasActuales);
//        }

//        if (vidasActuales <= 0)
//        {
//            GameOver();
//        }
//        else
//        {
//            Respawnear();
//        }
//    }

//    private void Respawnear()
//    {
//        if (rb != null)
//        {
//            rb.linearVelocity = Vector2.zero;
//        }

//        if (puntoRespawn != null)
//        {
//            transform.position = puntoRespawn.position;
//        }
//        else
//        {
//            transform.position = Vector3.zero;
//        }

//        // Teletransportar la cámara inmediatamente a la nueva posición del jugador
//        CamaraMinijuego camara = FindObjectOfType<CamaraMinijuego>();
//        if (camara != null)
//        {
//            camara.ResetearPosicion();
//        }

//        procesandoCaida = false;
//    }

//    private void GameOver()
//    {
//        juegoTerminado = true;
//        if (rb != null) rb.linearVelocity = Vector2.zero;

//        if (UIHUDMinijuego.Instancia != null)
//        {
//            UIHUDMinijuego.Instancia.MostrarGameOver();
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Meta") && !juegoTerminado)
//        {
//            juegoTerminado = true;
//            if (rb != null) rb.linearVelocity = Vector2.zero;

//            if (UIHUDMinijuego.Instancia != null)
//            {
//                UIHUDMinijuego.Instancia.MostrarVictoria();
//            }
//        }
//    }

//    private void OnDrawGizmosSelected()
//    {
//        if (puntoSuelo != null)
//        {
//            Gizmos.color = Color.green;
//            Gizmos.DrawWireSphere(puntoSuelo.position, radioSuelo);
//        }
//    }
//}

using UnityEngine;

public class JugadorMinijuego : MonoBehaviour
{
    [Header("Movimiento Horizontal")]
    public Rigidbody2D rb;
    public float velocidadHorizontal = 3f;

    [Header("Animator")]
    public Animator animator;

    [Header("Fuerza de Salto")]
    public float fuerzaSalto = 5f;  // Salto Corto
    public float fuerzaSalto2 = 8f;  // Salto Medio
    public float fuerzaSalto3 = 10f; // Salto Alto

    [Header("Detección de Suelo")]
    public Transform puntoSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    [Header("Sistema de Vidas y Respawn")]
    public int vidasMaximas = 3;
    private int vidasActuales;
    public Transform puntoRespawn;
    public float limiteCaidaY = -10f;

    private bool estaEnSuelo;
    public bool juegoTerminado = false;
    private bool procesandoCaida = false;

    private void Start()
    {
        vidasActuales = vidasMaximas;
        if (UIHUDMinijuego.Instancia != null)
        {
            UIHUDMinijuego.Instancia.ActualizarVidas(vidasActuales);
        }
    }

    private void Update()
    {
        if (juegoTerminado) return;

        if (transform.position.y < limiteCaidaY && !procesandoCaida)
        {
            CaerAlVacio();
        }
    }

    private void FixedUpdate()
    {
        if (juegoTerminado || rb == null)
            return;

        rb.linearVelocity = new Vector2(
            velocidadHorizontal,
            rb.linearVelocity.y
        );

        estaEnSuelo = Physics2D.OverlapCircle(
            puntoSuelo.position,
            radioSuelo,
            capaSuelo
        );

        ActualizarAnimacion();
    }

    private void ActualizarAnimacion()
    {
        if (animator != null)
        {
            animator.SetBool("Saltando", !estaEnSuelo);
        }
    }

    // --- Métodos de Salto ---

    public void Saltar() => EjecutarSalto(fuerzaSalto, "Salto Corto");
    public void Saltar2() => EjecutarSalto(fuerzaSalto2, "Salto Medio");
    public void Saltar3() => EjecutarSalto(fuerzaSalto3, "Salto Alto");

    private void EjecutarSalto(float fuerza, string logMensaje)
    {
        if (juegoTerminado || rb == null || !estaEnSuelo) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * fuerza, ForceMode2D.Impulse);
    }

    // --- Lógica de Daño, Caída y Meta ---

    private void CaerAlVacio()
    {
        procesandoCaida = true;
        vidasActuales--;

        if (UIHUDMinijuego.Instancia != null)
        {
            UIHUDMinijuego.Instancia.ActualizarVidas(vidasActuales);
        }

        if (vidasActuales <= 0)
        {
            GameOver();
        }
        else
        {
            Respawnear();
        }
    }

    //private void Respawnear()
    //{
    //    // 1. Resetear física
    //    if (rb != null)
    //    {
    //        rb.linearVelocity = Vector2.zero;
    //        rb.angularVelocity = 0f;
    //    }

    //    // 2. Mover posición
    //    if (puntoRespawn != null)
    //    {
    //        transform.position = puntoRespawn.position;
    //    }
    //    else
    //    {
    //        transform.position = Vector3.zero;
    //    }

    //    // 3. Resetear animación e visibilidad
    //    if (animator != null)
    //    {
    //        animator.Rebind();
    //        animator.Update(0f);
    //        animator.SetBool("Saltando", false);
    //    }

    //    // 4. Resetear cámara
    //    CamaraMinijuego camara = FindObjectOfType<CamaraMinijuego>();
    //    if (camara != null)
    //    {
    //        camara.ResetearPosicion();
    //    }

    //    procesandoCaida = false;
    //}

    private void Respawnear()
    {
        // 1. Resetear física
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = true;
        }

        // 2. Mover posición (asegurando Z = 0 y actualizando rb.position)
        Vector3 nuevaPos = (puntoRespawn != null)
            ? new Vector3(puntoRespawn.position.x, puntoRespawn.position.y, 0f)
            : Vector3.zero;

        transform.position = nuevaPos;
        if (rb != null) rb.position = nuevaPos;

        // 3. Resetear animación
        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
            animator.SetBool("Saltando", false);
        }

        // 4. Resetear cámara
        CamaraMinijuego camara = FindObjectOfType<CamaraMinijuego>();
        if (camara != null)
        {
            camara.ResetearPosicion();
        }

        procesandoCaida = false;
    }

    private void GameOver()
    {
        juegoTerminado = true;
        if (rb != null) rb.linearVelocity = Vector2.zero;

        if (UIHUDMinijuego.Instancia != null)
        {
            UIHUDMinijuego.Instancia.MostrarGameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Meta") && !juegoTerminado)
        {
            juegoTerminado = true;
            if (rb != null) rb.linearVelocity = Vector2.zero;

            if (UIHUDMinijuego.Instancia != null)
            {
                UIHUDMinijuego.Instancia.MostrarVictoria();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (puntoSuelo != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(puntoSuelo.position, radioSuelo);
        }
    }
}