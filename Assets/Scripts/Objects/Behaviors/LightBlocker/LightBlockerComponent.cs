using UnityEngine;

public class LightBlockerComponent : MonoBehaviour, IObjectBehavior
{
    private ILightBlocker behavior;

    public void Configure(LevelObjectParameters data)
    {
        behavior = data.lightBlockType == LightBlockType.OPAQUE ? new OpaqueBehavior() : new TransparentBehavior();
    }

    public bool BlocksLight(Vector2Int origin, Vector2Int target) => behavior.BlocksLight(origin, target);
}

public enum LightBlockType
{
    OPAQUE,
    TRANSPARENT
}