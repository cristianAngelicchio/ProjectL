using UnityEngine;

public class TransparentBehavior : ILightBlocker
{
    public bool BlocksLight(Vector2Int origin, Vector2Int target) => false;
}
