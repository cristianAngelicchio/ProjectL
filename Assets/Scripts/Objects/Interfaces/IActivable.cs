public interface IActivable
{
    bool IsActivated { get; }
    void Activate();
    void Deactivate();
}
