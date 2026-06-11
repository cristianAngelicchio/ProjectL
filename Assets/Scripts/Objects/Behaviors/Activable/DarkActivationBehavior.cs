public class DarkActivationBehavior : IActivationBehavior
{
    public bool IsActive { get; private set; }

    public void CheckActivation(ActivationData data)
    {
        IsActive = !data.tile.IsIlluminated;
    }
}
