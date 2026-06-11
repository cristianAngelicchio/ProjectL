using System;
using UnityEngine;

public class ActivableComponent : MonoBehaviour, IObjectBehavior
{
    public Action<ActivationData> Activate;
    private IActivationBehavior activationBehavior;

    public void Configure(LevelObjectParameters data)
    {
        switch(data.activationType)
        {
            case ActivationType.LIGHT: activationBehavior = new LightActivationBehavior(); break;
            case ActivationType.DARK: activationBehavior = new DarkActivationBehavior(); break;
            case ActivationType.WEIGHT: activationBehavior = new WeightActivationBehavior(); break;
            case ActivationType.POWERED: activationBehavior = new PoweredActivationBehavior(); break;
            case ActivationType.NONE: default: activationBehavior = new NoActivationBehavior(); break;
        }
    }

    public bool IsActive => activationBehavior.IsActive;

    public void CheckActivation(ActivationData data)
    {
        activationBehavior.CheckActivation(data);
        if (IsActive) Activate?.Invoke(data);
    }
}

public enum ActivationType
{
    NONE,
    LIGHT,
    DARK,
    WEIGHT,
    POWERED
}
