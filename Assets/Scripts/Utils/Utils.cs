using UnityEngine;

public static class Utils
{
    public static float RotationFromVector2(Vector2 direction)
    {
        return Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
    }
}
