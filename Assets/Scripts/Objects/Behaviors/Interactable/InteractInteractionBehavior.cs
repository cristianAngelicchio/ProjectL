using System;

public class InteractInteractionBehavior : IInteractableBehavior
{
    public Action<InteractionData> OnInteract;
    public void PerformInteraction(InteractionData data)
    {
        OnInteract?.Invoke(data);
    }
}