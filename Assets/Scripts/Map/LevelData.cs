using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Game/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    [Header("Map Settings")]
    public int width = 10;
    public int height = 10;
    public float tileSize = 1f;
    public Vector2 origin = Vector3.zero;

    [Header("Player Settings")]
    public Vector2Int playerSpawnPosition = Vector2Int.zero;
    public Direction playerSpawnDirection = Direction.NORTH;

    [Header("Level Objects")]
    public List<LevelObjectData> levelObjects = new List<LevelObjectData>();

    public Vector2 GetWorldSpawnPosition()
    {
        return origin + playerSpawnPosition;
    }
}
