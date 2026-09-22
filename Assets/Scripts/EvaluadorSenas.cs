using UnityEngine;

public static class EvaluadorSenas
{
    private static float Distancia(Vector2 a, Vector2 b)
    {
        return Vector2.Distance(a, b);
    }

    private static bool DedoExtendido(Vector2[] puntos, int tipIndex, int pipIndex)
    {
        return Distancia(puntos[tipIndex], Vector2.zero) > Distancia(puntos[pipIndex], Vector2.zero);
    }

    // 1. SEÑA: "I LOVE YOU"
    public static bool EsILoveYou(Vector2[] mano)
    {
        bool pulgar = DedoExtendido(mano, 4, 2);
        bool indice = DedoExtendido(mano, 8, 6);
        bool medio = DedoExtendido(mano, 12, 10);
        bool anular = DedoExtendido(mano, 16, 14);
        bool menique = DedoExtendido(mano, 20, 18);

        return pulgar && indice && !medio && !anular && menique;
    }

    // 2. SEÑA: "NO"
    public static bool EsSenaNo(Vector2[] mano)
    {
        float distPulgarIndice = Distancia(mano[4], mano[8]);
        float distIndiceMedio = Distancia(mano[8], mano[12]);

        bool pinzaJunta = (distPulgarIndice < 0.08f) && (distIndiceMedio < 0.08f);

        bool anularDoblado = !DedoExtendido(mano, 16, 14);
        bool meniqueDoblado = !DedoExtendido(mano, 20, 18);

        return pinzaJunta && anularDoblado && meniqueDoblado;
    }

    // 3. SEÑA: "FAMILIA"
    public static bool EsFamilia(Vector2[] mano)
    {
        float distPulgarIndice = Distancia(mano[4], mano[8]);

        bool circuloFormado = distPulgarIndice < 0.07f;
        bool medio = DedoExtendido(mano, 12, 10);
        bool anular = DedoExtendido(mano, 16, 14);
        bool menique = DedoExtendido(mano, 20, 18);

        return circuloFormado && medio && anular && menique;
    }
}