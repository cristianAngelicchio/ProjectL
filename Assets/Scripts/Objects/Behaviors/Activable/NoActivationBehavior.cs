public class NoActivationBehavior : IActivationBehavior
{
    public bool IsActive => false;

    public void CheckActivation(ActivationData data) {}
}
