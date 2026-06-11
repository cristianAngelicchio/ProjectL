using UnityEngine;

public class MovableComponent : MonoBehaviour, IObjectBehavior
{
    private IMovableBehavior movableBehavior;
    private Tile currentTile;
    private TileLayer layer;

    public void Configure(LevelObjectParameters data)
    {
        movableBehavior = data.movableType == MovableType.IMMOVABLE ? new ImmovableBehavior() : new MovableBehavior();
    }

    public bool CanMove(MovementData data)
    {
        return movableBehavior.CanMove(data);
    }

    public void Move(MovementData data)
    {
        movableBehavior.Move(data);
    }

    public void SetCurrentTile(Tile tile, TileLayer layer)
    {
        currentTile = tile;
        this.layer = layer;
    }
}

public enum MovableType
{
    IMMOVABLE,
    MOVABLE
}

public enum MovementType
{
    SWIMMING,
    WALKING,
    HOVERING
}