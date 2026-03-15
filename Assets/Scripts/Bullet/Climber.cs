using System;
using UnityEngine;

public class Climber : MonoBehaviour, ILaunchable
{    
    private Rigidbody2D _rb;        
    private WallBase _attachedWall;

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

        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;        
        
        _rb.AddForce(velocity, ForceMode2D.Impulse);

        gameObject.tag = "Untagged";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Wall")) return;
         
        WallBase wall = collision.gameObject.GetComponent<WallBase>();
        if(wall != null && _attachedWall == null)
        {
            _attachedWall = wall;

            _attachedWall.OnWallActived += WallActived;
            _attachedWall.OnWallDeactived += WallDeactived;

            _attachedWall.ActivateWall();
            
            AttachWall(collision.gameObject);
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Wall")) return;
        
        // Деактивация при отрыве
        WallBase wall = collision.gameObject.GetComponent<WallBase>();
        if(wall != null && wall == _attachedWall)
        {
            
            DetachWall();
        }
        
    }

    private void AttachWall(GameObject wall)
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0;
        _rb.bodyType = RigidbodyType2D.Kinematic;

        transform.parent = wall.transform;

        gameObject.tag = "ActiveBird";
    }

    private void DetachWall()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;

        _attachedWall.OnWallActived -= WallActived;
        _attachedWall.OnWallDeactived -= WallDeactived;

        _attachedWall.DeactivateWall(); // Возврат в исходное положение
        _attachedWall = null;

        _rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;
        gameObject.tag = "Untagged";
    }    

    private void WallActived()
    {
        Debug.Log("активация звука и тд, запуск можно сделать через события");
    }

    private void WallDeactived()
    {
        Debug.Log("активация звука и может быть тряска камеры");
    }

    public void Reset()
    {
        if (_attachedWall != null)
        {
            _attachedWall.OnWallActived -= WallActived;
            _attachedWall.OnWallDeactived -= WallDeactived;
            _attachedWall = null;
        }

        _rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;
        gameObject.tag = "Untagged";
    }

    private void OnDestroy()
    {
       Reset();
    }
}
