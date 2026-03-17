using System;
using DG.Tweening;
using UnityEngine;

public class WallTrampoline : WallBase
{
    public event Action OnRicochet;

    protected override void Awake()
    {
        base.Awake();
        _wallName = "WallTrampoline";
        _wallType = WallType.Trampoline;
    }

    public override void ActivateWall()
    {
        base.ActivateWall();        
        OnRicochet?.Invoke();
    }

}