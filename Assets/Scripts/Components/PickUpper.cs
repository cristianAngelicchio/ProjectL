using UnityEngine;

public class PickUpper : ObjectHandler<IInteractableBehavior>
{
    [SerializeField] private Transform playerTransform;

    private bool hasPickup;
    private IInteractableBehavior currentPickup;
    private LevelObject currentPickupObject;

    public bool HasPickup => hasPickup;
    public override bool HasPriority => hasPickup;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {


        if (hasPickup)
            DropObject(force: true);
    }

    public override void Act()
    {
        if (hasPickup)
        {
            DropObject();
        }
        else
        {
            // Obtener el objeto en el tile que el jugador está mirando
            IInteractableBehavior grababble = GetObjectOnFacingTile();

            if (grababble == null)
            {
                Debug.LogWarning("[PickUpper] No grabbable object on facing tile");
                return;
            }

            GrabObject(grababble);
        }
    }

    private void GrabObject(IInteractableBehavior grababble)
    {

    }

    private void DropObject(bool force = false)
    {
        if (currentPickup == null)
            return;

 
    }

    private void OnPlayerTileChanged(Vector2Int oldTile, Vector2Int newTile)
    {
        
    }
}
