public interface IMovableBehavior
{
    bool CanMove(MovementData data);
    void Move(MovementData data);
}
