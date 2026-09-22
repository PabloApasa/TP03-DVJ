using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // <--- 1. Agregamos la librería de TextMeshPro

[System.Serializable]
public struct SenaItem
{
    public string idSena;       // "ILOVEYOU", "NO", "FAMILIA"
    public string nombreAMostrar;
    public Sprite imagenSena;
}

public class GameManager : MonoBehaviour
{
    [Header("Referencias de UI")]
    public TMP_Text textoProfesora; // <--- 2. Cambiamos 'Text' por 'TMP_Text'
    public Image imagenPizarron;
    public TMP_Text textoFeedback;  // <--- 3. Cambiamos 'Text' por 'TMP_Text'

    [Header("Lista de Señas a Enseñar")]
    public List<SenaItem> listaSenas;

    private int indiceActual = 0;
    private bool esperandoEntrada = false;

    void Start()
    {
        if (textoFeedback != null) textoFeedback.text = "";
        CargarSenaActual();
    }

    void Update()
    {
        // SIMULACIÓN TECLADO:
        // Presiona 1 para "I LOVE YOU", 2 para "NO", 3 para "FAMILIA"
        if (esperandoEntrada)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) OnSenaDetectada("ILOVEYOU");
            if (Input.GetKeyDown(KeyCode.Alpha2)) OnSenaDetectada("NO");
            if (Input.GetKeyDown(KeyCode.Alpha3)) OnSenaDetectada("FAMILIA");
        }
    }

    void CargarSenaActual()
    {
        if (indiceActual < listaSenas.Count)
        {
            SenaItem sena = listaSenas[indiceActual];
            textoProfesora.text = "¡Hola! Hoy aprenderemos la seña: " + sena.nombreAMostrar;
            imagenPizarron.sprite = sena.imagenSena;
            esperandoEntrada = true;
        }
        else
        {
            textoProfesora.text = "¡Excelente trabajo! Has completado las 3 señas.";
            imagenPizarron.gameObject.SetActive(false);
            textoFeedback.text = "¡JUEGO COMPLETADO!";
        }
    }

    public void OnSenaDetectada(string idSena)
    {
        if (!esperandoEntrada) return;

        if (idSena == listaSenas[indiceActual].idSena)
        {
            StartCoroutine(RutinaSenaCorrecta());
        }
    }

    public void ProcesarLandmarksMediaPipe(Vector2[] puntosMano)
    {
        if (!esperandoEntrada) return;

        string senaActualTarget = listaSenas[indiceActual].idSena;

        if (senaActualTarget == "ILOVEYOU" && EvaluadorSenas.EsILoveYou(puntosMano))
        {
            OnSenaDetectada("ILOVEYOU");
        }
        else if (senaActualTarget == "NO" && EvaluadorSenas.EsSenaNo(puntosMano))
        {
            OnSenaDetectada("NO");
        }
        else if (senaActualTarget == "FAMILIA" && EvaluadorSenas.EsFamilia(puntosMano))
        {
            OnSenaDetectada("FAMILIA");
        }
    }

    IEnumerator RutinaSenaCorrecta()
    {
        esperandoEntrada = false;
        textoFeedback.text = "¡CORRECTO!";
        yield return new WaitForSeconds(2.0f);
        textoFeedback.text = "";

        indiceActual++;
        CargarSenaActual();
    }
}