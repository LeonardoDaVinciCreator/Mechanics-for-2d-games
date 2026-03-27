using System;
using DG.Tweening;
using UnityEngine;

public class Killer : MonoBehaviour, ILaunchable
{
    private Rigidbody2D _rb;

    private Tweener _currentTween;

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

        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;        
        
        _rb.AddForce(velocity, ForceMode2D.Impulse);

        gameObject.tag = "Player";
    }

    public void Reset()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        transform.position = Vector3.zero;
        gameObject.tag = "ActiveBird";
    }
}