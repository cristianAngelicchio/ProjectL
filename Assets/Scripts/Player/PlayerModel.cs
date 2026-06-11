using UnityEngine;

public class PlayerModel
{
    private Movement movement;
    private ObjectHandler<IInteractable> interactor;

    public PlayerModel(Movement movement = null, ObjectHandler<IInteractable> interactor = null)
    {
        this.movement = movement;
        this.interactor = interactor;
    }
}
