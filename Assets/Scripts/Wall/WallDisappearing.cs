using DG.Tweening;
using UnityEngine;

public class WallDisappearing : WallBase
{       

    private Collider2D _wallCollider;
    private SpriteRenderer _spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        _wallCollider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void ActivateWall()
    {       
        base.ActivateWall();
        gameObject.GetComponent<SpriteRenderer>().DOFade(0, _duration).OnComplete (
            () =>
            {                
                _wallCollider.enabled = false;
                DeactivateWall();
            }
        );;
        /*transform.DOScale(Vector3.one * 0.5f, _duration).OnComplete(() =>
        {
            DeactivateWall();
        });*/
    }

    
}