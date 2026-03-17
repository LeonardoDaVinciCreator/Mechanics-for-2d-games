using System;
using UnityEngine;

public class Climber : MonoBehaviour, ILaunchable
{    
    private Rigidbody2D _rb;        
    private WallBase _attachedWall;
    private WallTrampoline _trampolineWall;

    private Vector2 _lastVelocityBeforeCollision;

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

    private void FixedUpdate()
    {        
        //для сохранения скорости до столкновения, тк при столкновении сила пропадает
        _lastVelocityBeforeCollision = _rb.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Wall")) return;

        WallBase wall = collision.gameObject.GetComponent<WallBase>();

        if (wall is WallTrampoline)
        {            
            Vector2 incomingVelocity = _lastVelocityBeforeCollision;            
            if (incomingVelocity.sqrMagnitude < 0.01f) 
                incomingVelocity = _rb.linearVelocity;

            Vector2 normal = collision.contacts[0].normal;

            //теперь нормаль сморит на игрока
            if (Vector2.Dot(incomingVelocity, normal) > 0)
            {
                normal = -normal;
            }

            Vector2 finalVelocity;

            if(incomingVelocity.y< 0)
            {
                Vector2 reflectedVelocity = Vector2.Reflect(incomingVelocity, normal);                 
                finalVelocity = reflectedVelocity;
            }
            else
            {
                Vector2 reflectedVelocity = Vector2.Reflect(incomingVelocity, normal);           
            
                finalVelocity = reflectedVelocity + (Vector2.up * 5);//для поднятие вверх
            }

            _rb.linearVelocity = finalVelocity;

            float inAngle = Mathf.Atan2(incomingVelocity.y, incomingVelocity.x) * Mathf.Rad2Deg;
            float outAngle = Mathf.Atan2(finalVelocity.y, finalVelocity.x) * Mathf.Rad2Deg;

            Debug.Log($"inAngle: {inAngle:F1}  normal: {normal}");
            Debug.Log($"outAngle: {outAngle:F1} (finalVelocity: {finalVelocity:F2})");            

            return;
        }        

        _trampolineWall = wall as WallTrampoline;
        if(wall != null && _attachedWall == null)
        {
            _attachedWall = wall;

            _attachedWall.OnWallActived += WallActived;
            _attachedWall.OnWallDeactived += WallDeactived;

            if (_trampolineWall != null)
                _trampolineWall.OnRicochet += WallRicochet;


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

        if (_trampolineWall != null)
            _trampolineWall.OnRicochet -= WallRicochet;

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

    private void WallRicochet()
    {
        Debug.Log("запуск звука и тд");
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
