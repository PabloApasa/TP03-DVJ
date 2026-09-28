using UnityEngine;

public class ProfesoraTTS : MonoBehaviour
{
    [Header("Audio de la profesora")]
    public AudioSource audioSource;

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

        // Si ya estaba hablando, detenemos el audio anterior
        audioSource.Stop();

        // Reproducimos el nuevo
        audioSource.PlayOneShot(audio);
    }

    public void Saludar()
    {
        Hablar(saludo);
    }

    public void DarInstruccion()
    {
        Hablar(instruccion);
    }


    public void Animar()
    {
        Hablar(casi);
    }

    public void DarAyuda()
    {
        Hablar(ayuda);
    }

    public void Siguiente()
    {
        Hablar(siguienteSena);
    }

    public void Finalizar()
    {
        Hablar(final);
    }
}