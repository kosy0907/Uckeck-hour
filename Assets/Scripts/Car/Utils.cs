using UnityEngine;

public class Utils
{
    private static float SetCeil(float num, float carHalfSize)
    {
        if (num % 1 == 0 && carHalfSize == 2)
        {
            if (num >= 0)
            {
                return num - 0.5f;
            }
            return num + 0.5f;

        }
        return num;
    }
    // Round to nearest half step to fix inaccuracies in vectors
    public static Vector3 RoundVector(Vector3 v, float carHalfSize)
    {
        return new Vector3(
          Utils.SetCeil(Mathf.Round(v.x * 2) / 2, carHalfSize),
         Mathf.Round(v.y * 2) / 2,
          Utils.SetCeil(Mathf.Round(v.z * 2) / 2, carHalfSize)
        );
    }
}
