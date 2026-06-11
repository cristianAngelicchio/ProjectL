using System.Collections.Generic;
using UnityEngine;

public class SquareLightEmmiterBehavior : ILightEmitterBehavior
{
    private int range;
    public SquareLightEmmiterBehavior(int r) { range = r; }

    public IEnumerable<Vector2Int> GetIlluminatedTiles(Vector2Int origin, Direction direction)
    {
        for (int i = -range; i <= range; i++)
        { 
            for (int j = -range; j <= range; j++)
            {
                yield return origin + new Vector2Int(i, j);
            }
        }
    }
}
