using System;
using UnityEngine;

public class Climber : MonoBehaviour, ILaunchable
{    
    private Rigidbody2D _rb;        

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        transform.parent = null;
        _rb.WakeUp();
        _rb.useFullKinematicContacts = true;
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;      
    }

    public void Launch(Vector2 velocity)
    {
        transform.parent = null;

        /* _rb.simulated = false;
        _rb.simulated = true; */

        _rb.isKinematic = false;
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;        
        
        _rb.AddForce(velocity, ForceMode2D.Impulse);

        gameObject.tag = "Untagged";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Wall")) return;
         
        AttachWall(collision.gameObject);
    }

    private void AttachWall(GameObject wall)
    {
        /* var wallRb = wall.GetComponent<Rigidbody2D>();
        if (wallRb == null) return; */

        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0;
        _rb.isKinematic = true;
        //_rb.simulated = true;


        transform.parent = wall.transform;

        gameObject.tag = "ActiveBird";
    }

    public void Reset()
    {
        _rb.isKinematic = false;
        transform.parent = null;

       /*  _joint.enabled = false; */

        // как пример
        // если минимальное расстояние, то нужно вернуть на исходную позицию
    }
}
