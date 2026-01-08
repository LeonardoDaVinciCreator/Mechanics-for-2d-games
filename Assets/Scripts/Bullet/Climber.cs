using System;
using UnityEngine;

public class Climber : MonoBehaviour, ILaunchable
{    
    private Rigidbody2D _rb;
    [SerializeField]
    private FixedJoint2D _joint;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.WakeUp();
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _joint.enabled = false;
        _joint.enableCollision = true;
        _joint.breakAction = JointBreakAction2D.Disable;
    }

    public void Launch(Vector2 velocity)
    {
        //if (_joint != null)
        //{
        //    _joint.enabled = false;
        //    Destroy(_joint);
        //    _joint = null;
        //}
        //_rb.simulated = false;
        //_rb.simulated = true;

        _joint.enabled = false;
        _joint.connectedBody = null;

        _rb.simulated = false;
        _rb.simulated = true;

        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        _rb.isKinematic = false;
        _rb.linearVelocity = Vector2.zero;
        _rb.AddForce(velocity, ForceMode2D.Impulse);

        gameObject.tag = "Untagged";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Wall")) return;
         
        AttachWall(collision.gameObject);

        //при выстеле нужно отсоединять от joint или вообще не использовать, только риджибда
    }

    private void AttachWall(GameObject wall)
    {
        var wallRb = wall.GetComponent<Rigidbody2D>();
        if (wallRb == null) return;

        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0;

        //_joint = gameObject.AddComponent<FixedJoint2D>();//лучше сделать так: не добавлять, а менять connectedBody
        _joint.connectedBody = wallRb;
        _joint.enabled = true;

        gameObject.tag = "ActiveBird";
    }

    public void Reset()
    {
        _rb.isKinematic = false;

        _joint.enabled = false;

        // как пример
        // если минимальное расстояние, то нужно вернуть на исходную позицию
    }
}
