public class MovableBehavior : IMovableBehavior
{
    public bool CanMove(MovementData data)
    {
        return true;
    }

    public void Move(MovementData data)
    {
        //remove from current tile
        //call events
        //move object
        //add to new tile
        //call events
    }
}
