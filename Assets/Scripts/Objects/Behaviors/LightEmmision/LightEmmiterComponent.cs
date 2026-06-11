using UnityEngine;

public class LightEmmiterComponent : MonoBehaviour, IObjectBehavior
{
    private ILightEmitterBehavior emitter;
    private int range;

    public void Configure(LevelObjectParameters data)
    {
        range = data.lightRange;
        switch (data.lightShape)
        {
            case LightShape.CONE:
                emitter = new ConeLightEmmiterBehavior(range);
                break;
            case LightShape.AREA:
                emitter = new SquareLightEmmiterBehavior(range);
                break;
            case LightShape.LINE:
                emitter = new LineLightEmmiterBehavior(range);
                break;
            case LightShape.NONE:
            default:
                emitter = new NoLightEmmiterBehavior();
                break;
        }
    }

    public void ApplyIllumination(Tile originTile, GridMap map, Direction orientation)
    {
        var tiles = emitter.GetIlluminatedTiles(originTile.GridPosition, orientation);
        foreach (var pos in tiles)
        {
            var tile = map.GetTile(pos);
            if (tile != null)
                tile.AddLightSource(this);
        }
    }

    public void RemoveIllumination(Tile originTile, GridMap map, Direction orientation)
    {
        var tiles = emitter.GetIlluminatedTiles(originTile.GridPosition, orientation);
        foreach (var pos in tiles)
        {
            var tile = map.GetTile(pos);
            if (tile != null)
                tile.RemoveLightSource(this);
        }
    }
}

public enum LightShape
{
    NONE,
    CONE,
    AREA,
    LINE
}
