using DG.Tweening;
using NUnit.Framework;
using UnityEngine;

public class WallDisappearing : WallBase
{
    private Collider2D _wallCollider;
    private SpriteRenderer _spriteRenderer;
    private Tweener _fadeTween;

    protected override void Awake()
    {
        base.Awake();
        _wallCollider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    override public void ActivateWall()
    {       
        base.ActivateWall();

        _fadeTween?.Kill();

        _fadeTween = _spriteRenderer.DOFade(0, 1/_speed).OnComplete(() =>
        {
            _wallCollider.enabled = false;
            DeactivateWall();
        });            
    }

    override public void DeactivateWall()
    {
        //возврат к изначальному свойству объекта
        if(!IsActive) return;

        _fadeTween?.Kill();
        _spriteRenderer.DOFade(1, 1/_speed).OnComplete(() =>
        {
            _wallCollider.enabled = true;           
        });
    }    
}