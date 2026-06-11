using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectHandler<T> : MonoBehaviour
{
    [SerializeField] protected GridMap gridMap;

    protected List<T> objectsNearby = new List<T>();

    public virtual bool HasPriority => false;

    public void TryAddObject(Collider collider)
    {
        if (!collider.TryGetComponent<T>(out var obj))
            return;

        if (objectsNearby.Contains(obj))
            return;

        objectsNearby.Add(obj);
    }

    public void TryRemoveObject(Collider collider)
    {
        if (!collider.TryGetComponent<T>(out var obj))
            return;

        if (!objectsNearby.Contains(obj))
            return;

        objectsNearby.Remove(obj);
    }

    public abstract void Act();

    public bool HasObjectsNearby()
    {
        return objectsNearby.Count > 0;
    }

    protected T GetObjectOnFacingTile()
    {
        return default(T);
    }
}
