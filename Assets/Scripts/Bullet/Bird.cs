using UnityEngine;

public class Bird : MonoBehaviour, ILaunchable
{
    private Rigidbody2D rb;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    public void Launch(Vector2 velocity)
    {
        rb.isKinematic = false;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(velocity, ForceMode2D.Impulse);
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;
        gameObject.tag = "Untagged";
    }

    public void Reset()
    {
        rb.isKinematic = false;
        // Вернуть на исходную позицию или уничтожить
        //Destroy(gameObject);
    }
}
