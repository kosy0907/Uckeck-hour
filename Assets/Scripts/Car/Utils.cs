using UnityEngine;

public class Utils
{
    // Round to nearest half step to fix inaccuracies in vectors
    public static Vector3 RoundVector(Vector3 v)
    {
        return new Vector3(
          Mathf.Round(v.x),
          Mathf.Round(v.y),
          Mathf.Round(v.z)
        );
    }
}
