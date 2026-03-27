using UnityEngine;

public class Bird : MonoBehaviour, ILaunchable
{
    private Rigidbody2D _rb;

    private void Awake() => _rb = GetComponent<Rigidbody2D>();

    public void Launch(Vector2 velocity)
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(velocity, ForceMode2D.Impulse);
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;
        gameObject.tag = "Player";
    }

    public void Reset()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        // Вернуть на исходную позицию или уничтожить
        //Destroy(gameObject);
    }
}
