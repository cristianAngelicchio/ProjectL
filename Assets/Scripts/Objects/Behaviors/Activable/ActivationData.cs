public struct ActivationData
{
    public LevelObject obj;
    public Tile tile;
    public bool poweredSource;

    public ActivationData(LevelObject o, Tile t, bool powered)
    {
        obj = o;
        tile = t;
        poweredSource = powered;
    }
}
