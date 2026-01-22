using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Класс для определения обычного одинарного свайпа
/// 
/// 
/// пк/геймпада только Vector2
/// для определения свайпа для мышки/тачскрина Vector2 + button
/// 
/// также нужен клик для остановки
/// </summary>
public class SwipeDetectorBase : MonoBehaviour
{
    public static SwipeDetectorBase Instance
    {
        get;
        private set;
    }

    public static event Action<Vector2> OnSwipeDetected;

    [Header("Settings")]
    [SerializeField]
    protected float _swipeDistance = 100f;    

    [Header("Actions")]
    [SerializeField]
    protected InputActionReference _swipeAction;//позиция на экране
    [SerializeField]
    protected InputActionReference _touchAction;//нажатие на экран

    protected Vector2 _startPosition;
    protected Vector2 _currentPosition;
    protected bool _isTouching;

    private void OnEnable()
    {
        _swipeAction.action.Enable();
        _swipeAction.action.performed += OnSwipe;

        _touchAction.action.Enable();
        _touchAction.action.started += OnTouchStart;
        _touchAction.action.canceled += OnTouchEnd;
    }

    private void OnDisable()
    {
        _swipeAction.action.performed -= OnSwipe;
        _swipeAction.action.Disable();
        
        _touchAction.action.started -= OnTouchStart;
        _touchAction.action.canceled -= OnTouchEnd;
        _touchAction.action.Disable();        
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }        
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTouchStart(InputAction.CallbackContext context)
    {
        _isTouching = true;
        _startPosition = _currentPosition;
    }        

    private void OnTouchEnd(InputAction.CallbackContext context)
    {
        if(!_isTouching) return;
        _isTouching = false;

        //направление свайпа
        Vector2 delta = _currentPosition - _startPosition;
        if(delta.magnitude > _swipeDistance)
        {
            Vector2 direction = delta.normalized;
            float angle = Vector2.SignedAngle(Vector2.left, direction);  
            
            switch (angle)
            {
                case float n when (n >= -45f && n <= 45f):
                    Debug.Log("направо");
                    break;
                case float n when (n > 45f && n <= 135f):
                    Debug.Log("Вверх");
                    break;
                case float n when (n > 135f || n <= -135f):
                    Debug.Log("налево");
                    break;
                default:
                    Debug.Log("Вниз");
                    break;
            }


            Debug.Log($"Направл: {direction}, Угол: {angle:F0}, ");

            OnSwipeDetected?.Invoke(direction);
        }
    }

    private void OnSwipe(InputAction.CallbackContext position)
    {
        _currentPosition = position.ReadValue<Vector2>();
    }
}
