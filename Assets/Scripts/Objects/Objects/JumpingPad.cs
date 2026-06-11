public class JumpingPad : LevelObject
{
    private bool isEnabled;

    protected override void InitializeBehaviors()
    {
        base.InitializeBehaviors();

        var actComponent = GetBehavior<ActivableComponent>();
        var stateComponent = GetBehavior<StateComponent>();

        actComponent.Activate += MoveObject;
        stateComponent.OnTurnOff += () => isEnabled = false;
        stateComponent.OnTurnOn += () => isEnabled = true;
    }

    private void MoveObject(ActivationData data)
    {
        if (!isEnabled) return;

        var objectIntile = data.tile.GetObject(TileLayer.OBJECT);
        if (!objectIntile) return;
        MovementData movData = new MovementData
        {
            obj = objectIntile,
            originTile = data.tile
        };

        var movComponent = objectIntile.GetBehavior<MovableComponent>();

        if (movComponent.CanMove(movData)) movComponent.Move(movData);
    }
}
