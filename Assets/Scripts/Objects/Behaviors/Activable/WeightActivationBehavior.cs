public class WeightActivationBehavior : IActivationBehavior
{
    public bool IsActive { get; private set; }

    public void CheckActivation(ActivationData data)
    {
        IsActive = !data.tile.IsEmpty(TileLayer.OBJECT);
    }
}
