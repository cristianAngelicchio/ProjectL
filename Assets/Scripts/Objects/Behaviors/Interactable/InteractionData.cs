using UnityEngine;

[System.Serializable]
public struct InteractionData
{
    public Transform PlayerTransform;
    public Vector2Int PlayerTile;
    public Vector2Int TargetTile;

    public static InteractionData Create(
        Transform playerTransform,
        Vector2Int playerTile,
        Vector2Int targetTile)
    {
        return new InteractionData
        {
            PlayerTransform = playerTransform,
            PlayerTile = playerTile,
            TargetTile = targetTile
        };
    }

    public bool IsValid()
    {
        return PlayerTransform != null;
    }

    public override string ToString()
    {
        return $"[InteractionData] Player: {PlayerTile} → Target: {TargetTile}";
    }
}