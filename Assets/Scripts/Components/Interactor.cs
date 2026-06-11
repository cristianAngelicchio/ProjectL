using UnityEngine;

public class Interactor : ObjectHandler<IInteractableBehavior>
{
    [SerializeField] private Transform playerTransform;

    public override void Act()
    {
        IInteractableBehavior interactable = GetObjectOnFacingTile();

        InteractionData interactionData = InteractionData.Create(
            playerTransform: playerTransform,
            playerTile: new Vector2Int(),
            targetTile: new Vector2Int()
        );
    }
}
