using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class StealthController : SlingshotControllerBase
{
    [Header("Stealth")]
    [SerializeField]
    private float _stealthRadius = 1.5f;
    [SerializeField]
    private InputActionReference _interactionAction;
    [SerializeField]
    private RadiusKiller _radiusKiller;

    private bool _isInteracting => _radiusKiller != null ? _radiusKiller.CanInteract : false;
    private Vector2 _climbAnchor;     

    override protected void OnEnable()
    {
        base.OnEnable();

        _interactionAction.action.Enable();
        _interactionAction.action.performed += OnInteraction;
    }

    override protected void OnDisable()
    {
        base.OnDisable();

        _interactionAction.action.performed -= OnInteraction;
        _interactionAction.action.Disable();
    }      

    private void OnInteraction(InputAction.CallbackContext context)
    {
        if(!_isInteracting) return;

        Debug.Log("Intraction");
    }

    protected override void AttachTarget()
    {
        //base.AttachTarget();

        _climbAnchor = _targetObject.transform.position;
        _centerSlingshot.position = _climbAnchor;
    }

    protected override void PullTarget(Vector2 screenPosition)
    {
        if (_targetObject == null) return;

        Debug.Log("Плавное движение киллера");

        //положение мышки на экране
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, -
            Camera.main.transform.position.z));

        _distance = Vector2.Distance(worldPos, _climbAnchor);        

        _finalDistance = Mathf.Clamp(_distance, 0, _maxDistance);
    }

    override protected void OnAttachEnd(InputAction.CallbackContext context)
    {
        _isPulling = false;
        if (_targetObject != null && _launcher != null)
        {
            _finalDistance = Mathf.Clamp(_finalDistance, 0, _maxDistance);
            if (_finalDistance >= _minDistance)
            {                
                Vector2 velocity = CalculateVelocity();
                _launcher.Launch(velocity);
            }
            else
            {                
                _launcher.Reset();
            }           
        }        
    }

    public override Vector2 CalculateVelocity()
    {
        Vector2 direction = new Vector2(
            Mathf.Cos(_angle * Mathf.Deg2Rad),
            Mathf.Sin(_angle * Mathf.Deg2Rad)
        );

        return direction
            * Mathf.Clamp(_finalDistance, _minDistance, _maxDistance)
            * _forceMultiplier;
    }
}