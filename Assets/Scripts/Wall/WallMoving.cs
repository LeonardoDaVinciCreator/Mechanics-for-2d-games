using DG.Tweening;
using UnityEngine;

public class WallMoving : WallBase
{
    [Header("Movement Limits")]    
    [SerializeField]
    private float _distance = 1f;    

    private Vector3 _originalPosition;
    private Tweener _moveTween;//анимация
    [SerializeField]
    private LoopType _type;

    protected override void Awake()
    {
        base.Awake();
        _wallType = WallType.Moving;
    }

    private void Start()
    {
        _originalPosition = transform.position;
    }

    public override void ActivateWall()
    {
        base.ActivateWall();
        _moveTween?.Kill();//уничтожение анимаций как для корутин
        MoveWall();
    }
    public override void DeactivateWall()
    {
        base.DeactivateWall();
        _moveTween?.Kill();
        transform.DOMove(_originalPosition, 1);//возврат к изначальному положению
    }

    private void MoveWall()
    {
        Vector3 targetPos = _originalPosition + (Vector3)Vector2.up * _distance;

        _moveTween = transform.DOMove(targetPos, 1/_speed).SetLoops(-1, _type);
    }
}