using UnityEngine;

public class SwipeController : MonoBehaviour
{
    void OnEnable()
    {        
        SwipeDetectorBase swipeDetectorBase = SwipeDetectorBase.Instance;
        SwipeDetectorBase.OnSwipeDetected += HandleSwipe;
    }

    void OnDisable()
    {
        SwipeDetectorBase.OnSwipeDetected -= HandleSwipe;
    }


    private void HandleSwipe(Vector2 direction)
    {
        Debug.Log($"Свайп: {direction}");
    }
}