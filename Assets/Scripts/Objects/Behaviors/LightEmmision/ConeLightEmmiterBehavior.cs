using System.Collections.Generic;
using UnityEngine;

public class ConeLightEmmiterBehavior : ILightEmitterBehavior
{
    private int range;
    public ConeLightEmmiterBehavior(int r) { range = r; }

    public IEnumerable<Vector2Int> GetIlluminatedTiles(Vector2Int origin, Direction direction)
    {
        Vector2Int forward = direction.ToVector2Int();

        for (int i = 1; i <= range; i++)
        {
            Vector2Int center = origin + forward * i;
            yield return center;

            if (forward.x != 0)
            {
                yield return center + Vector2Int.up * i;
                yield return center + Vector2Int.down * i;
            }
            else if (forward.y != 0)
            {
                yield return center + Vector2Int.left * i;
                yield return center + Vector2Int.right * i;
            }
        }
    }
}
