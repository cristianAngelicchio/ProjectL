using System.Collections.Generic;
using UnityEngine;

public class NoLightEmmiterBehavior : ILightEmitterBehavior
{
    public IEnumerable<Vector2Int> GetIlluminatedTiles(Vector2Int origin, Direction direction)
    {
        return new List<Vector2Int>();
    }
}
