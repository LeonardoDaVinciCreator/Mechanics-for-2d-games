using UnityEngine;

public class SwipeDetectorHorizontalOrVecrtical : SwipeDetectorBase
{
    protected override Vector2 FinalDirection(Vector2 direction)
    {
        float absX = Mathf.Abs(direction.x);//положительное значение
        float absY = Mathf.Abs(direction.y);

        if(absX > absY)
        {
            if(direction.x > 0)
            {
                Debug.Log("налево");
                return Directions.Left;
            }
            else
            {
                Debug.Log("направо");
                return Directions.Right;
            }
        }
        else
        {
            if(direction.y > 0)
            {
                Debug.Log("вниз");
                return Directions.Down;
            }
            else
            {
                Debug.Log("вверх");
                   return Directions.Up;
            }
        }            
    }
}
