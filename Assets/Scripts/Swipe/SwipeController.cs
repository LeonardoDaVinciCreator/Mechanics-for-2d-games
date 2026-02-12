using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SwipeController : MonoBehaviour
{    
    [SerializeField] 
    private float _speed = 10;

    Rigidbody2D _rb;
    private Light2D _light;

    void OnEnable()
    {        
        SwipeDetectorBase swipeDetectorBase = SwipeDetectorBase.Instance;
        SwipeDetectorBase.OnSwipeDetected += HandleSwipe;
        SwipeDetectorBase.OnClickDetected += StopObject;
    }

    void OnDisable()
    {
        SwipeDetectorBase.OnSwipeDetected -= HandleSwipe;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _light = GetComponentInChildren<Light2D>();        
    }

    private void HandleSwipe(Vector2 direction)
    {
       _rb.linearVelocity = direction * _speed; 
       if (_light != null)
            RotateLight(direction);
    }

    private void StopObject()
    {
        _rb.linearVelocity = Vector2.zero; // Остановка!
    }

    private void RotateLight(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        _light.transform.rotation = Quaternion.Euler(0, 0, angle);
    } 
}