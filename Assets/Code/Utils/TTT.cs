using System.Collections;
using UnityEngine;

/// <summary>
/// 2D To 3D by ignoring the y.
/// </summary>
public static class TTT
{
    public static float Distance(Vector3 left, Vector3 right)
    {
        left.y = right.y = 0;
        return Vector3.Distance(right, left);
    }

    public static Vector3 Direction(Vector3 to, Vector3 from)
    {
        from.y = to.y = 0;
        return (to - from).normalized;
    }
}
