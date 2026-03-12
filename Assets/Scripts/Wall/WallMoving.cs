using DG.Tweening;
using UnityEngine;

public class WallMoving : WallBase
{
    [Header("Movement Limits")]
    [SerializeField]
    private float _duration = 0.5f;
    [SerializeField]
    private float _distance = 1f;

    private Vector3 _originalPosition;
    private Tweener _moveTween;//анимация

    protected void Awake()
    {
        base.Awake();
        _wallName = "WallMoving";
        _wallType = WallType.Moving;
    }

    private void Start()
    {
        _originalPosition = transform.position;
    }

    override public void ActivateWall()
    {
        base.ActivateWall();
        _moveTween?.Kill();//уничтожение анимаций как для корутин
        MoveWall();
    }
    public override void DeactivateWall()
    {
        base.DeactivateWall();
        _moveTween?.Kill();
        transform.DOMove(_originalPosition, _duration);//возврат к изначальному положению
    }

    private void MoveWall()
    {
        Vector3 targetPos = _originalPosition + (Vector3)Vector2.up * _distance;

        _moveTween = transform.DOMove(targetPos, _duration).SetLoops(-1, LoopType.Yoyo);
    }
}