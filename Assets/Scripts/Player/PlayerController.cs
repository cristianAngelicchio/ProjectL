using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : LevelObject
{
    [SerializeField] private Movement movement;
    [SerializeField] private Interactor interactor;
    [SerializeField] private PickUpper pickUpper;
    [SerializeField] private CollisionHandler collisionHandler;
    [SerializeField] private TriggerHandler triggerHandler;
    private InputSystemActions input;
    private PlayerModel model;

    private void Awake()
    {
        model = new PlayerModel(movement, null);
        input = new InputSystemActions();
    }

    public override void Initialize(LevelObjectData data, GridMap gridMap)
    {
        base.Initialize(data, gridMap);
        input.Enable();
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;
        input.Player.Interact.started += OnInteract;

        if (triggerHandler != null)
        {
            triggerHandler.OnTriggerEnterHandler += HandleTriggerEnter;
            triggerHandler.OnTriggerExitHandler += HandleTriggerExit;
        }
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMove;
        input.Player.Interact.started -= OnInteract;

        if (triggerHandler != null)
        {
            triggerHandler.OnTriggerEnterHandler -= HandleTriggerEnter;
            triggerHandler.OnTriggerExitHandler -= HandleTriggerExit;
        }
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 direction = ctx.ReadValue<Vector2>();
        Vector3 movementDirection = new Vector3(direction.x, 0, direction.y);
        movement.SetMovement(movementDirection);
        movement.SetRotation(movementDirection);
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        // Si tiene prioridad (objeto agarrado), usar PickUpper
        if (pickUpper != null && pickUpper.HasPriority)
        {
            pickUpper.Act();
            return;
        }

        // Intentar interactuar primero
        if (interactor != null && interactor.HasObjectsNearby())
        {
            interactor.Act();
            return;
        }

        // Fallback: intentar agarrar
        if (pickUpper != null && pickUpper.HasObjectsNearby())
        {
            pickUpper.Act();
            return;
        }

        Debug.LogWarning("[PlayerController] No interactive or grabbable object on facing tile");
    }

    private void HandleTriggerEnter(Collider other)
    {
        if (interactor != null)
            interactor.TryAddObject(other);
        if (pickUpper != null)
            pickUpper.TryAddObject(other);
    }

    private void HandleTriggerExit(Collider other)
    {
        if (interactor != null)
            interactor.TryRemoveObject(other);
        if (pickUpper != null)
            pickUpper.TryRemoveObject(other);
    }
}
