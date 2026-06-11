using UnityEngine;

public class ColliderComponent : MonoBehaviour, IObjectBehavior
{

    private IColliderBehavior colliderBehavior;
    public void Configure(LevelObjectParameters data)
    {
        switch (data.colliderLevel)
        {
            case ColliderLevel.LOW:
                colliderBehavior = new LowColliderBehavior();
                break;
            case ColliderLevel.BASE:
                colliderBehavior = new BaseColliderBehavior();
                break;
            case ColliderLevel.HIGH:
                colliderBehavior = new HighColliderBehavior();
                break;
        }
    }

    public bool CanCollide(MovementType type)
    {
        return colliderBehavior != null && colliderBehavior.CanCollide(type);
    }
}

public enum ColliderLevel
{
    LOW,
    BASE,
    HIGH
}