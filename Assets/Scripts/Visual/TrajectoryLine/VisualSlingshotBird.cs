using UnityEngine;

public class VisualSlingshotBird : TrajectoryLineBase
{
    [Header("Slingshot Bird")]
    [SerializeField]
    private Transform _birdTransform;

    protected override void UpdateTrajectoryLine()
    {
        if(_slingshotController == null || _birdTransform == null)
            return;
        
        Vector2 startPosition = _birdTransform.position;
        Vector2 velocity = _slingshotController.CalculateVelocity();

        Vector2 currentPos = startPosition;
        int maxIterations = 100; // Лимит, чтобы не лагало
        float timeStep = Time.fixedDeltaTime;// ~0.02f
        Vector2 gravity = Physics2D.gravity;
        _trajectoryLine.positionCount = _pointsCount;

        _trajectoryLine.SetPosition(0, currentPos);
        for (int i = 1; i < _pointsCount && i < maxIterations; i++)
        {
            // Симуляция физики (гравитация)
            
            currentPos += velocity * timeStep;
            velocity += gravity * timeStep;
            _trajectoryLine.SetPosition(i, currentPos);

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

    public void SetBird(Transform bird) => _birdTransform = bird;
   
}