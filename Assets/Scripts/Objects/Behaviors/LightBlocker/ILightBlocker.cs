using UnityEngine;

public interface ILightBlocker
{
    bool BlocksLight(Vector2Int origin, Vector2Int target);
}
