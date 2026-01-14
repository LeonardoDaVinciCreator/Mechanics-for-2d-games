using UnityEngine;
using UnityEngine.InputSystem;

public class SlingshotController : SlingshotControllerBase
{
    protected override void AttachTarget()
    {
       _targetObject.transform.position = _centerSlingshot.position;
        //ShowVisuals(false);

        _visual.ShowVisuals(true);
    }    

    private void LaunchBird(float angle, float distance)
    {        
        Rigidbody2D rb = _targetObject.GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.linearVelocity = Vector2.zero;

        Vector2 launchDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector2 launchVelocity = launchDirection * Mathf.Clamp(distance, _minDistance, _maxDistance) * _forceMultiplier;

        rb.AddForce(launchVelocity, ForceMode2D.Impulse);
    }

    private void ResetSlingshot()
    {
        if(_targetObject != null)
        {
            _targetObject.GetComponent<Rigidbody2D>().isKinematic = false;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(_attachScreenPosition.x, _attachScreenPosition.y, -Camera.main.transform.position.z));
            _targetObject.transform.position = worldPos;
        }
    }

    protected override void PullTarget(Vector2 screenPos)
    {
        base.PullTarget(screenPos);
    }

    public override Vector2 CalculateVelocity()
    {
        Vector2 direction = new Vector2(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad));
        return direction * Mathf.Clamp(_finalDistance, _minDistance, _maxDistance) * _forceMultiplier;
    }
}
