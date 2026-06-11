using UnityEngine;

public class Platform : LevelObject
{
    public GameObject unlitPlatform;
    public GameObject litPlatform;
    protected override void InitializeBehaviors()
    {
        base.InitializeBehaviors();
        OnPlaceOnTile += AddEvents;
        OnRemoveFromTile += RemoveEvents;
    }

    private void AddEvents()
    {
        currentTile.OnIlluminationChanged += OnIlluminationChanged;
        OnIlluminationChanged(currentTile, currentTile.IsIlluminated);
    }

    private void RemoveEvents()
    {
        currentTile.OnIlluminationChanged -= OnIlluminationChanged;
    }

    private void OnIlluminationChanged(Tile tile, bool isLit)
    {
        unlitPlatform.SetActive(!isLit);
        litPlatform.SetActive(isLit);
    }

    private void OnDestroy()
    {
        RemoveEvents();
        OnPlaceOnTile -= AddEvents;
        OnRemoveFromTile -= RemoveEvents;
    }
}
