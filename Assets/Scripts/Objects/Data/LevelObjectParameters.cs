using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelObjectParameters
{
    [Header("Movement")]
    public MovableType movableType;
    public MovementType movementType;
    public ColliderLevel colliderLevel;

    [Header("Size")]
    public int width = 1;
    public int length = 1;
    public bool isEdgeObject;

    [Header("Interaction")]
    public InteractionType interactionType;

    [Header("Light Emmision")]
    public LightShape lightShape;
    public int lightRange;

    [Header("Light Block")]
    public LightBlockType lightBlockType;

    [Header("Activable")]
    public ActivationType activationType;
    public bool isPowered;
    public List<string> linkedElements = new List<string>();

    [Header("States")]
    public bool hasStates;
    public bool initialState;
}
