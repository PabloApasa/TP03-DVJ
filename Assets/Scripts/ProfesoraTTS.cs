using UnityEngine;

public class ProfesoraTTS : MonoBehaviour
{
    [Header("Audio de la profesora")]
    public AudioSource audioSource;

    [Header("Animator de la profesora")]
    public Animator animator;

    [Header("Frases")]
    public AudioClip saludo;
    public AudioClip instruccion;
    public AudioClip casi;
    public AudioClip ayuda;
    public AudioClip siguienteSena;
    public AudioClip final;


    public void Hablar(AudioClip audio)
    {
        if (audio == null)
        {
            Debug.LogWarning("No se asignó un audio.");
            return;
        }

        if (audioSource == null)
        {
            Debug.LogWarning("No se asignó el AudioSource.");
            return;
        }

        audioSource.Stop();
        audioSource.PlayOneShot(audio);
    }


    // SALUDO
    public void Saludar()
    {
        Hablar(saludo);

        if (animator != null)
            animator.SetTrigger("Saludar");
    }


    // EXPLICACIÓN / INSTRUCCIÓN
    public void DarInstruccion()
    {
        Hablar(instruccion);

        if (animator != null)
            animator.SetTrigger("Explanation");
    }


    // CASI
    public void Animar()
    {
        Hablar(casi);

        if (animator != null)
            animator.SetTrigger("Wrong");
    }


    // AYUDA
    public void DarAyuda()
    {
        Hablar(ayuda);

        if (animator != null)
            animator.SetTrigger("Wrong");
    }


    // CORRECTO
    public void Siguiente()
    {
        Hablar(siguienteSena);

        if (animator != null)
            animator.SetTrigger("Congratulation");
    }


    // FINAL
    public void Finalizar()
    {
        Hablar(final);

        if (animator != null)
            animator.SetTrigger("Congratulation");
    }
}