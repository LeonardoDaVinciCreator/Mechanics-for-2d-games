using UnityEngine;

public enum SlidingWallDirection
{
    Up,
    Down,
    Left,
    Right
}

public class WallSliding : WallBase
{
    [SerializeField]
    private SlidingWallDirection _slideDirection;
    public SlidingWallDirection SlideDirection => _slideDirection;

    override public void ActivateWall()
    {
        base.ActivateWall();    
    }
}