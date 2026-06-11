using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ObjectCatalog", menuName = "Game/Object Catalog")]
public class ObjectCatalog : ScriptableObject
{
    public CatalogEntry playerEntry;
    public List<CatalogEntry> entries;
}

[System.Serializable]
public class CatalogEntry
{
    public GameObject prefab;
    public Sprite previewSprite;
    public LevelObjectParameters defaultParameters;
}
