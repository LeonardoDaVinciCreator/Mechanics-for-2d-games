using UnityEngine;

public abstract class InteractObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        OnInteract();
    }

    protected abstract void OnInteract();
}
