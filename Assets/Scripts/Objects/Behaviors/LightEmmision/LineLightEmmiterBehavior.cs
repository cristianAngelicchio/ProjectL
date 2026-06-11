using System.Collections.Generic;
using UnityEngine;

public class LineLightEmmiterBehavior : ILightEmitterBehavior
{
    private int range;
    public LineLightEmmiterBehavior(int r) { range = r; }

    public IEnumerable<Vector2Int> GetIlluminatedTiles(Vector2Int origin, Direction direction)
    {
        Vector2Int dir = direction.ToVector2Int();
        for (int i = 0; i <= range; i++)
        {
            yield return origin + dir * i;
        }
    }
}
