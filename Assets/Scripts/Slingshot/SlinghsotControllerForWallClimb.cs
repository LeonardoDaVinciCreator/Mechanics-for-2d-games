using UnityEngine;

/* 
добавить визуал скалолоза обновленный
 */

public class SlinghsotControllerForWallClimb : SlingshotControllerBase
{
    private Vector2 _climbAnchor;

    protected override void AttachTarget()
    {        
        _climbAnchor = _targetObject.transform.position;
        _centerSlingshot.position = _climbAnchor;

        /* ShowVisuals(false); */
    }

    protected override void PullTarget(Vector2 screenPosition)
    {
        if (_targetObject == null) return;

        Debug.Log("Плавное движение скалолоза");

        //положение мышки на экране
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPosition.x, screenPosition.y, -
            Camera.main.transform.position.z));

        _distance = Vector2.Distance(worldPos, _climbAnchor);        

        _finalDistance = Mathf.Clamp(_distance, 0, _maxDistance);

        /* ShowVisuals(true);
        UpdateTrajectory();
        UpdateCircles(); */
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
