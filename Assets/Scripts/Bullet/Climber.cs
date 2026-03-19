using System;
using DG.Tweening;
using UnityEngine;

public class Climber : MonoBehaviour, ILaunchable
{    
    private Rigidbody2D _rb;        
    private WallBase _attachedWall;
    private WallTrampoline _trampolineWall;

    private Tweener _currentTween;

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
        if (_attachedWall != null)
        {
            DetachWall();
        }

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

        if (_attachedWall != null) return;

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

        
        if(wall != null)
        {
            _attachedWall = wall;
            _trampolineWall = wall as WallTrampoline;

            _attachedWall.OnWallActived += WallActived;
            _attachedWall.OnWallDeactived += WallDeactived;

            if (_trampolineWall != null)
                _trampolineWall.OnRicochet += WallRicochet;


            _attachedWall.ActivateWall();
            
            AttachWall(collision.gameObject);

            if(wall is WallSliding)
            {
                AttachSlidingWall(wall);
            }
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

    private void AttachSlidingWall(WallBase wall)
    {
        WallSliding wallSliding = (WallSliding)wall;
        Vector2 position = transform.localPosition;
        Vector2 size = wall.GetComponent<BoxCollider2D>().size;
        float speed = wallSliding.Speed;
        Vector2 pos = Vector2.zero;
        switch (wallSliding.SlideDirection)
        {
            case SlidingWallDirection.Up:
                pos = new Vector2(position.x, size.y/2);
                break;
            case SlidingWallDirection.Down:
                pos = new Vector2(position.x, -size.y/2);
                break;
            case SlidingWallDirection.Left:
                pos = new Vector2(-size.x/2, position.y);
                break;
            case SlidingWallDirection.Right:
                pos = new Vector2(size.x/2, position.y);
                break;            
        }
        SlideClimber(pos, speed);
    }

    private void SlideClimber(Vector2 direction, float speed)
    {
        if (_attachedWall == null) return;        
        
        _currentTween = transform.DOLocalMove(direction, 1/speed).OnComplete(
            () =>
            {
                if (this != null && gameObject != null)
                {
                    DetachWall();
                }
            }
        );
    }

    private void DetachWall()
    {        
        if (_attachedWall == null) return;

        _currentTween?.Kill();
        _currentTween = null;

        WallBase wallToDetach = _attachedWall;
        WallTrampoline trampolineToDetach = _trampolineWall;

        Vector2 worldPosition = transform.position;
        Quaternion worldRotation = transform.rotation;
        
        if (wallToDetach != null && wallToDetach.gameObject != null)
        {            
            wallToDetach.OnWallActived -= WallActived;
            wallToDetach.OnWallDeactived -= WallDeactived;
            
            if (wallToDetach.gameObject.activeInHierarchy)
            {
                wallToDetach.DeactivateWall();
            }
        }

        if (trampolineToDetach != null)
        {
            trampolineToDetach.OnRicochet -= WallRicochet;
        }

        transform.parent = null;
        transform.SetPositionAndRotation(worldPosition, worldRotation);
        gameObject.tag = "Untagged";
                
        _rb.bodyType = RigidbodyType2D.Dynamic;

        _attachedWall = null;
        _trampolineWall = null;
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
