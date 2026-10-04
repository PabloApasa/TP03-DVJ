using System.Collections;
using UnityEngine;

public class TutorialControles : MonoBehaviour
{
    [Header("Imágenes del Tutorial")]
    [Tooltip("Arrastra aquí ÚNICAMENTE las imágenes que forman parte de la secuencia.")]
    [SerializeField] private GameObject[] imagenesAsignadas;

    [Header("Tiempos")]
    [Tooltip("Tiempo que se muestra cada imagen antes de pasar a la siguiente.")]
    [SerializeField] private float tiempoPorImagen = 1.0f;

    [Tooltip("Tiempo total antes de ocultar las imágenes y finalizar el tutorial (12 a 15 segundos).")]
    [SerializeField] private float duracionTotal = 15.0f;

    private void OnEnable()
    {
        if (imagenesAsignadas != null && imagenesAsignadas.Length > 0)
        {
            StartCoroutine(CicloSecuenciaCo());
        }
    }

    private IEnumerator CicloSecuenciaCo()
    {
        float tiempoTranscurrido = 0f;
        int indiceActual = 0;

        // Desactiva solo las imágenes asignadas al inicio
        DesactivarSoloAsignadas();

        while (tiempoTranscurrido < duracionTotal)
        {
            // Apaga solo las imágenes de la lista y enciende la actual
            DesactivarSoloAsignadas();
            if (imagenesAsignadas[indiceActual] != null)
            {
                imagenesAsignadas[indiceActual].SetActive(true);
            }

            // Espera el tiempo configurado por imagen
            yield return new WaitForSeconds(tiempoPorImagen);
            tiempoTranscurrido += tiempoPorImagen;

            // Pasa a la siguiente imagen de la lista (vuelve a la primera al llegar al final)
            indiceActual = (indiceActual + 1) % imagenesAsignadas.Length;
        }

        // Al cumplirse el tiempo total, apaga únicamente las imágenes asignadas
        DesactivarSoloAsignadas();
    }

    private void DesactivarSoloAsignadas()
    {
        foreach (GameObject img in imagenesAsignadas)
        {
            if (img != null)
            {
                img.SetActive(false);
            }
        }
    }
}