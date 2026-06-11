using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Vector2Int GridPosition { get; private set; }
    public Vector3 WorldPosition { get; private set; }

    public event Action<Tile, bool> OnIlluminationChanged;

    public bool IsIlluminated => lightEmitters.Count > 0;

    private Dictionary<TileLayer, LevelObject> contents;
    private HashSet<LightEmmiterComponent> lightEmitters;

    public Tile(Vector2Int gridPos, Vector3 worldPos)
    {
        GridPosition = gridPos;
        WorldPosition = worldPos;
        lightEmitters = new HashSet<LightEmmiterComponent>();
        contents = new Dictionary<TileLayer, LevelObject>();
    }

    public bool TryPlaceObject(TileLayer layer, LevelObject obj)
    {
        if (contents.ContainsKey(layer)) return false;

        contents[layer] = obj;
        obj.PlaceOnTile(this);
        return true;
    }

    public bool RemoveObject(TileLayer layer)
    {
        if (!contents.ContainsKey(layer)) return false;

        var obj = contents[layer];
        contents.Remove(layer);
        obj.RemoveFromTile(this);
        return true;
    }

    public LevelObject GetObject(TileLayer layer)
    {
        contents.TryGetValue(layer, out var obj);
        return obj;
    }

    public bool IsEmpty(TileLayer layer) => !contents.ContainsKey(layer) || contents[layer] is null;

    public void AddLightSource(LightEmmiterComponent light)
    {
        if (lightEmitters.Add(light) && lightEmitters.Count == 1)
        {
            OnIlluminationChanged?.Invoke(this, true);
        }
    }

    public void RemoveLightSource(LightEmmiterComponent light)
    {
        if (lightEmitters.Remove(light) && lightEmitters.Count == 0)
        {
            OnIlluminationChanged?.Invoke(this, false);
        }
    }
}