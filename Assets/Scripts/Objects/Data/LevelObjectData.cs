using UnityEngine;

[System.Serializable]
public class LevelObjectData
{
    [Header("General")]
    public string objectId;
    public GameObject prefab;

    [Header("Grid Settings")]
    public Vector2Int gridPosition;
    public TileLayer layer;

    [Header("Orientation")]
    public Direction orientation;

    [Header("Parameters")]
    public LevelObjectParameters parameters;
}