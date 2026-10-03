
using UnityEngine;

public class JugadorMinijuego : MonoBehaviour
{
    [Header("Movimiento Horizontal")]
    public Rigidbody2D rb;
    public float velocidadHorizontal = 3f;

    [Header("Animator")]
    public Animator animator;

    [Header("Fuerza de Salto")]
    public float fuerzaSalto = 5f;
    public float fuerzaSalto2 = 8f;
    public float fuerzaSalto3 = 10f;

    [Header("Detección de Suelo")]
    public Transform puntoSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    private bool estaEnSuelo;


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


    public void Saltar()
    {
        if (rb == null)
            return;

        if (!estaEnSuelo)
        {
            Debug.Log("No puede saltar porque está en el aire.");
            return;
        }

        rb.AddForce(
            Vector2.up * fuerzaSalto,
            ForceMode2D.Impulse
        );

        Debug.Log("¡El personaje saltó!");
    }


    public void Saltar2()
    {
        if (rb == null)
            return;

        if (!estaEnSuelo)
        {
            Debug.Log("No puede saltar porque está en el aire.");
            return;
        }

        rb.AddForce(
            Vector2.up * fuerzaSalto2,
            ForceMode2D.Impulse
        );

        Debug.Log("¡El personaje saltó2!");
    }


    public void Saltar3()
    {
        if (rb == null)
            return;

        if (!estaEnSuelo)
        {
            Debug.Log("No puede saltar porque está en el aire.");
            return;
        }

        rb.AddForce(
            Vector2.up * fuerzaSalto3,
            ForceMode2D.Impulse
        );

        Debug.Log("¡El personaje saltó3!");
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
