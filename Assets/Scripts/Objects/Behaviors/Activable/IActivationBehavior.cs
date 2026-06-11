public interface IActivationBehavior
{
    void CheckActivation(ActivationData data);
    bool IsActive { get; }
}
