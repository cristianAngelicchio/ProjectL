using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;

    private Vector3 movementDirection;
    private Vector3 lookDirection;

    public void SetMovement(Vector3 direction)
    {
        movementDirection = direction;
    }

    public void SetRotation(Vector3 direction)
    {
        lookDirection = direction;
    }

    private void Update()
    {
        if (IsPhysicallyMovable())
            return;
        
        SimultateMovement(Time.deltaTime, false);
    }
    
    private void FixedUpdate()
    {
        if (!IsPhysicallyMovable())
            return;

        SimultateMovement(Time.fixedDeltaTime, true);
    }

    private bool IsPhysicallyMovable()
    {
        if (!rigidbody)
            return false;

        return !rigidbody.isKinematic;
    }

    private void SimultateMovement(float deltaTime, bool moveWithPhysics)
    {
        if (movementDirection.sqrMagnitude > 1f)
            movementDirection.Normalize();
        
        Move(deltaTime, moveWithPhysics);
        Rotate(deltaTime, moveWithPhysics);
    }

    private void Move(float deltaTime, bool moveWithPhysics)
    {
        Vector3 movement = movementSpeed * deltaTime * movementDirection;
        if (moveWithPhysics)
            rigidbody.MovePosition(rigidbody.position + movement);
        else
            transform.Translate(movement, Space.World);
    }
    
    private void Rotate(float deltaTime, bool moveWithPhysics)
    {
        Vector3 dir = RotationDirection();
        if (dir == Vector3.zero)
            return;
        
        Quaternion targetRotation = Quaternion.LookRotation(dir);
        float maxDegrees = rotationSpeed * 100 * deltaTime;

        if (moveWithPhysics)
        {
            rigidbody.MoveRotation(Quaternion.RotateTowards(rigidbody.rotation, targetRotation, maxDegrees));
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegrees);
        }
    }

    private Vector3 RotationDirection()
    {
        if (lookDirection.sqrMagnitude > 0.001f)
            return lookDirection.normalized;
        
        return movementDirection.sqrMagnitude > 0.001f ? movementDirection.normalized : Vector3.zero;
    }
}
