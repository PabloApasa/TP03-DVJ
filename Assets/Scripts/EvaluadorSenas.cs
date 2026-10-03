using UnityEngine;

public static class EvaluadorSenas
{
    /// <summary>
    /// Devuelve la distancia escalar de la palma (muñeca p[0] a base del medio p[9]) 
    /// para usar como referencia de escala dinámica.
    /// </summary>
    private static float ObtenerEscalaPalma(Vector2[] p)
    {
        return Vector2.Distance(p[0], p[9]);
    }

    /// <summary>
    /// Comprueba si un dedo largo (índice, medio, anular o meñique) está extendido 
    /// verificando que su punta esté más alejada de la muñeca que su articulación media (PIP).
    /// </summary>
    private static bool DedoEstaExtendido(Vector2[] p, int indicePunta, int indicePIP)
    {
        return p[indicePunta].magnitude > p[indicePIP].magnitude * 1.1f;
    }

    /// <summary>
    /// Comprueba si un dedo largo está doblado hacia la palma.
    /// </summary>
    private static bool DedoEstaDoblado(Vector2[] p, int indicePunta, int indiceMCP)
    {
        return p[indicePunta].magnitude < p[indiceMCP].magnitude * 1.05f;
    }

    // ==========================================================
    // 1. EVALUADOR "I LOVE YOU"
    // Puntas: Pulgar(4), Índice(8), Medio(12), Anular(16), Meñique(20)
    // Articulaciones intermedias: Índice PIP(6), Medio MCP(9), Anular MCP(13), Meñique PIP(18)
    // ==========================================================
    public static bool EsILoveYou(Vector2[] p)
    {
        if (p == null || p.Length < 21) return false;

        float escalaPalma = ObtenerEscalaPalma(p);
        if (escalaPalma < 0.05f) return false; // Mano muy lejana o no detectada correctamente

        // 1. EXTENDIDOS: Pulgar, Índice y Meñique
        bool indiceExtendido = DedoEstaExtendido(p, 8, 6);
        bool meniqueExtendido = DedoEstaExtendido(p, 20, 18);

        // Pulgar abierto: la punta del pulgar (p[4]) debe estar alejada del nudillo del índice (p[5])
        bool pulgarAbierto = Vector2.Distance(p[4], p[5]) > escalaPalma * 0.8f;

        // 2. DOBLADOS: Medio y Anular pegados a la palma
        bool medioDoblado = DedoEstaDoblado(p, 12, 9);
        bool anularDoblado = DedoEstaDoblado(p, 16, 13);

        // 3. SEPARACIÓN: Medio y Anular deben estar claramente más cortos que el índice y meñique
        bool medioMasCortoQueIndice = p[12].magnitude < p[8].magnitude * 0.75f;
        bool anularMasCortoQueMenique = p[16].magnitude < p[20].magnitude * 0.75f;

        return indiceExtendido && meniqueExtendido && pulgarAbierto &&
               medioDoblado && anularDoblado &&
               medioMasCortoQueIndice && anularMasCortoQueMenique;
    }

    // ==========================================================
    // 2. EVALUADOR "NO" (Pellizco de 3 dedos: Pulgar, Índice y Medio juntos)
    // Anular y Meñique deben permanecer más atrasados o doblados.
    // ==========================================================
    public static bool EsSenaNo(Vector2[] p)
    {
        if (p == null || p.Length < 21) return false;

        float escalaPalma = ObtenerEscalaPalma(p);
        if (escalaPalma < 0.05f) return false;

        // Distancias entre las 3 yemas
        float distPulgarIndice = Vector2.Distance(p[4], p[8]);
        float distIndiceMedio = Vector2.Distance(p[8], p[12]);
        float distPulgarMedio = Vector2.Distance(p[4], p[12]);

        // Umbral de contacto relativo a la palma
        float umbralContacto = escalaPalma * 0.40f;

        bool yemasJuntas = distPulgarIndice < umbralContacto &&
                           distIndiceMedio < umbralContacto &&
                           distPulgarMedio < umbralContacto;

        // Exclusión: Para evitar que confunda un puño cerrado con "NO", 
        // las yemas del trío no deben estar totalmente pegadas a la muñeca.
        bool yemasProyectadas = p[8].magnitude > escalaPalma * 0.8f;

        // Exclusión: Anular y Meñique no deben estar tocando al trío
        bool anularSeparado = Vector2.Distance(p[16], p[4]) > umbralContacto * 1.2f;

        return yemasJuntas && yemasProyectadas && anularSeparado;
    }

    // ==========================================================
    // 3. EVALUADOR "FAMILIA" / "OK"
    // Círculo entre Pulgar e Índice; Medio, Anular y Meñique BIEN extendidos.
    // ==========================================================
    public static bool EsFamilia(Vector2[] p)
    {
        if (p == null || p.Length < 21) return false;

        float escalaPalma = ObtenerEscalaPalma(p);
        if (escalaPalma < 0.05f) return false;

        // 1. CÍRCULO: Pulgar e Índice tocándose
        float distPulgarIndice = Vector2.Distance(p[4], p[8]);
        bool circuloFormado = distPulgarIndice < escalaPalma * 0.35f;

        // 2. EXTENDIDOS: Medio, Anular y Meñique erguidos
        bool medioExtendido = DedoEstaExtendido(p, 12, 10);
        bool anularExtendido = DedoEstaExtendido(p, 16, 14);
        bool meniqueExtendido = DedoEstaExtendido(p, 20, 18);

        // 3. EXCLUSIÓN: Medio, Anular y Meñique deben ser mucho más largos que el círculo
        float alturaCirculo = p[8].magnitude;
        bool dedosRestantesAltos = p[12].magnitude > alturaCirculo * 1.15f &&
                                   p[16].magnitude > alturaCirculo * 1.10f &&
                                   p[20].magnitude > alturaCirculo * 1.05f;

        return circuloFormado && medioExtendido && anularExtendido && meniqueExtendido && dedosRestantesAltos;
    }
}