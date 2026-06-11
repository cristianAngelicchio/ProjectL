using UnityEngine;

public class OpaqueBehavior : ILightBlocker
{
    public bool BlocksLight(Vector2Int origin, Vector2Int target) => true;
}
