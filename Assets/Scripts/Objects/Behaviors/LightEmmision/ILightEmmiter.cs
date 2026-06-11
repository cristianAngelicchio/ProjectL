using System.Collections.Generic;
using UnityEngine;

public interface ILightEmitterBehavior
{
    IEnumerable<Vector2Int> GetIlluminatedTiles(Vector2Int origin, Direction direction);
}