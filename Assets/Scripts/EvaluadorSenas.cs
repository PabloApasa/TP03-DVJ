using UnityEngine;

public static class EvaluadorSenas
{
    // Evaluador de "I LOVE YOU" (Índice y Meñique levantados; Medio y Anular más abajo)
    public static bool EsILoveYou(Vector2[] p)
    {
        if (p == null || p.Length < 21) return false;

        float dIndice = p[8].magnitude;
        float dMedio = p[12].magnitude;
        float dAnular = p[16].magnitude;
        float dMenique = p[20].magnitude;

        // Condición clave: El Índice y el Meñique deben estar más extendidos que el Medio y el Anular
        bool indiceMayorQueMedio = dIndice > dMedio * 1.1f;
        bool meniqueMayorQueAnular = dMenique > dAnular * 1.1f;

        return indiceMayorQueMedio && meniqueMayorQueAnular;
    }

    // Evaluador de "NO" (Las puntas de pulgar, índice y medio están muy cerca)
    public static bool EsSenaNo(Vector2[] p)
    {
        if (p == null || p.Length < 21) return false;

        float distPulgarIndice = Vector2.Distance(p[4], p[8]);
        float distIndiceMedio = Vector2.Distance(p[8], p[12]);

        return distPulgarIndice < 0.8f && distIndiceMedio < 0.8f;
    }

    // Evaluador de "FAMILIA" (Puntas de pulgar e índice juntas en "OK", el meñique extendido)
    public static bool EsFamilia(Vector2[] p)
    {
        if (p == null || p.Length < 21) return false;

        float distPulgarIndice = Vector2.Distance(p[4], p[8]);
        float dMenique = p[20].magnitude;
        float dMedio = p[12].magnitude;

        bool circuloFormado = distPulgarIndice < 0.7f;
        bool dedosRestantesArriba = dMenique > 0.6f && dMedio > 0.6f;

        return circuloFormado && dedosRestantesArriba;
    }
}