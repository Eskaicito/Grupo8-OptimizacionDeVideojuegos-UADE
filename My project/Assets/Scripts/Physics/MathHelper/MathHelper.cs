using UnityEngine;

public static class MathHelper
{
    public static void NormalizeSafe(ref Vector3 vector)
    {
        float mag = vector.magnitude;
        if (mag > 0.0001f)
        {
            vector /= mag;
        }
    }

    public static Vector3 RightFromForward(Vector3 forward)
    {
        return Vector3.Cross(Vector3.up, forward);
    }
}
