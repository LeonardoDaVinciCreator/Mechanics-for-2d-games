using UnityEngine;
using UnityEngine.InputSystem;

public abstract class SlingshotControllerBase : MonoBehaviour
{
    public static event System.Action<Bird> OnBirdLaunched;
    public static event System.Action OnSlingshotReset;

    [Header("Settings")]    
    [SerializeField]
    protected float _minDistance;
    [SerializeField]
    protected float _maxDistance;

    [Space(2)]
    [SerializeField]
    protected float _forceMultiplier = 0.5f;

    [Space(2)]
    [SerializeField]
    private float _minAngle;
    [SerializeField]
    private float _maxAngle;    

    [Header("Actions"), Space(10)]
    [SerializeField]
    private InputActionReference _attachAction;
    [SerializeField]
    private InputActionReference _pullPositionAction;    

    [Header("Visual"), Space(10)]    
    [SerializeField]
    protected Transform _centerSlingshot;
    [SerializeField]
    private int _pointsCount = 30;
    [SerializeField] 
    private LineRenderer _trajectoryLine;
    [SerializeField, Space(2)] 
    private LineRenderer _minCircle;
    [SerializeField] 
    private LineRenderer _maxCircle;

    protected bool _isPulling;
    protected float _distance;
    protected float _finalDistance;
    protected float _angle;

    protected Vector2 _attachScreenPosition;
    protected GameObject _targetObject;
    protected ILaunchable _launcher;


    private void OnEnable()
    {
        _attachAction.action.Enable();
        _attachAction.action.started += OnAttachStart;
        _attachAction.action.canceled += OnAttachEnd;

        _pullPositionAction.action.Enable();
        _pullPositionAction.action.performed += OnPull;        
    }

    private void OnDisable()
    {
        _attachAction.action.started -= OnAttachStart;
        _attachAction.action.canceled -= OnAttachEnd;
        _attachAction.action.Disable();

        _pullPositionAction.action.performed -= OnPull;
        _pullPositionAction.action.Disable();
    }

    private void Start()
    {
        SetupCircle(_minCircle, _minDistance);
        SetupCircle(_maxCircle, _maxDistance);
    }
    protected void OnAttachStart(InputAction.CallbackContext context)
    {
        _isPulling = true;
        _attachScreenPosition = _pullPositionAction.action.ReadValue<Vector2>();

        RaycastHit2D hit = GetRaycast(_attachScreenPosition);
        if (hit.collider?.CompareTag("ActiveBird") ?? false)
        {
            _targetObject = hit.collider.gameObject;
            Rigidbody2D rb = _targetObject.GetComponent<Rigidbody2D>();
            if (rb != null && !_targetObject.TryGetComponent<Climber>(out _))
            {
                rb.isKinematic = true;
            }

            _launcher = _targetObject.GetComponent<ILaunchable>();
            if (_launcher != null)
            {
                AttachTarget();
            }
        }        
    }

    protected virtual void OnAttachEnd(InputAction.CallbackContext context)
    {
        _isPulling = false;
        if (_targetObject != null && _launcher != null)
        {
            _finalDistance = Mathf.Clamp(_finalDistance, 0, _maxDistance);
            if (_finalDistance >= _minDistance)
            {                
                Vector2 velocity = CalculateVelocity();
                _launcher.Launch(velocity);
                OnBirdLaunched?.Invoke(_targetObject.GetComponent<Bird>());
            }
            else
            {
                //ResetSlingshot();
                _launcher.Reset();
                OnSlingshotReset?.Invoke();
            }
            _targetObject = null;
            _launcher = null;
        }
        ShowVisuals(false);
        
        //смена объекта: пока просто спавн такого же объекта для рогатки
    }

    private void SetupCircle(LineRenderer line, float radius)
    {
        if (line == null) return;

        line.positionCount = 32;//количество точек круга
        line.useWorldSpace = true;
        line.enabled = false;

        for (int i = 0; i < 32; i++)
        {
            float angle = i * Mathf.PI * 2f / _pointsCount;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            line.SetPosition(i, _centerSlingshot.position + (Vector3)offset);
        }
    }        

    protected void OnPull(InputAction.CallbackContext context)
    {
        if (_isPulling && _launcher != null)
        {
            Vector2 currentPosition = context.ReadValue<Vector2>();
            Vector2 delta = currentPosition - _attachScreenPosition;

            Vector2 direction = delta.normalized;//направление для подсчета угла
            _distance = delta.magnitude;//длина для подсчета силы натяжения
            _finalDistance = _distance;

            _angle = Vector2.SignedAngle(Vector2.left, direction);//выставление 0 слева
            _angle = Mathf.Clamp(_angle, _minAngle, _maxAngle);

            PullTarget(currentPosition);

            Debug.Log($"Направление: {direction}, Дистанция: {_distance}, Угол: {_angle:F0}");
        }        
    }

    protected virtual void PullTarget(Vector2 screenPosition)
    {
        if (_targetObject == null) return;

        Debug.Log("Плавное движение птицы");
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, -
            Camera.main.transform.position.z));
        _targetObject.transform.position = worldPos;

        ShowVisuals(true);
        UpdateTrajectory();
        UpdateCircles();
    }

    protected void UpdateTrajectory()
    {
        if (_trajectoryLine == null || _targetObject == null) return;
        _trajectoryLine.positionCount = _pointsCount;

        //точка выстрела
        
        Vector2 slingshotPos = _targetObject.transform.position;//центр или же берется фиксированная позиция на рогатке
        
        Vector2 velocity = CalculateVelocity();

        Vector2 currentPos = slingshotPos;
        float timeStep = Time.fixedDeltaTime; // ~0.02f, идеально для предсказания
        int maxIterations = 100; // Лимит, чтобы не лагало
        Vector2 gravity = Physics2D.gravity;

        _trajectoryLine.SetPosition(0, currentPos);

        for (int i = 0; i < _pointsCount && i < maxIterations; i++)
        {
            _trajectoryLine.SetPosition(i, currentPos);

            // Симуляция физики (гравитация)
            
            currentPos += velocity * timeStep;
            velocity += gravity * timeStep;

            // Проверка столкновений
            if (currentPos.y < -10f || currentPos.magnitude > 20f)
            {
                // Дополняем оставшиеся точки
                for (int j = i + 1; j < _pointsCount; j++)
                {
                    _trajectoryLine.SetPosition(j, currentPos);
                }
                break;
            }
        }
    }

    protected void UpdateCircles()
    {
        UpdateCirclePos(_minCircle, _minDistance);
        UpdateCirclePos(_maxCircle, _maxDistance);
    }

    private void UpdateCirclePos(LineRenderer line, float radius)
    {
        if (line == null) return;
        Vector2 center = _centerSlingshot.position;

        for (int i = 0; i < line.positionCount; i++)
        {
            float angle = i * Mathf.PI * 2f / line.positionCount;
            Vector2 point = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            line.SetPosition(i, point);
        }
    }

    protected void ShowVisuals(bool show)
    {
        if (_trajectoryLine) _trajectoryLine.enabled = show;
        if (_minCircle) _minCircle.enabled = show;
        if (_maxCircle) _maxCircle.enabled = show;
    }

    private bool IsTouchingBird(Vector2 screenPosition)
    {
        RaycastHit2D hit = GetRaycast(screenPosition);
        return hit.collider?.CompareTag("ActiveBird") ?? false;
    }

    private RaycastHit2D GetRaycast(Vector2 screenPosition)
    {
        Vector3 wordPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, -10f)
            );

        return Physics2D.Raycast(wordPosition, Vector2.zero);
    }

    protected virtual void AttachTarget()
    {
        Debug.Log("Прикрепление цели");
    }

    /// <summary>
    /// метод для расчета силы:
    /// изменять силу, вектор, направление относительно необходимого результата
    /// </summary>
    /// <returns></returns>
    protected abstract Vector2 CalculateVelocity();    
}
