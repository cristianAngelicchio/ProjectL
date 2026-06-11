public interface ITileContent
{
    void Initialize(LevelObjectData data, GridMap gridMap);
    void Rotate(Direction newDirection);
    void PlaceOnTile(Tile tile);
    void RemoveFromTile(Tile tile);
}
