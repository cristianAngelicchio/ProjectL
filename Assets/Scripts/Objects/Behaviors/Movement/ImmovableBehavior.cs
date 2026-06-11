public class ImmovableBehavior : IMovableBehavior
{
    public bool CanMove(MovementData data) => false;
    public void Move(MovementData data) {}
}
