public struct MovementData
{
    public LevelObject obj;
    public Tile originTile;
    public Tile targetTile;

    public MovementData(LevelObject o, Tile origin, Tile target)
    {
        obj = o;
        originTile = origin;
        targetTile = target;
    }
}