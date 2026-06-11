using UnityEngine;

public class PoweredActivationBehavior : IActivationBehavior
{
    public bool IsActive { get; private set; }

    public void CheckActivation(ActivationData data)
    {
        IsActive = data.poweredSource;
    }
}
