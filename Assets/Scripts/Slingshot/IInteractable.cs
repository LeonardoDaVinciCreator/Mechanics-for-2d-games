public interface IInteractable
{
    event System.Action OnCanInteractChanged;
    bool CanInteract { get; }
}