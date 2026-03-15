using System;
using Unity.VisualScripting;
using UnityEngine;

public enum WallType 
{ 
    Regular, 
    Moving, 
    Sliding, 
    Disappearing 
}

public class WallBase : MonoBehaviour
{
    [Header("Wall Settings")]
    [SerializeField]
    protected string _wallName = "Wall";    
    [SerializeField]
    protected WallType _wallType = WallType.Regular;
    
    [Header("Detection")]
    [SerializeField]
    protected float _radiusDetection;//радиус для колайдера обнаружения персонажа, если 0 => то не радиуса обнаружения, то есть проверка на спорикосновение к самой стене
    [SerializeField]
    protected float _duration;//можно использовать для усложнения уровня или для упрощения например ускорять или увеличивать
    private Collider2D _collider;

    public event Action OnWallActived;
    public event Action OnWallDeactived;
    public bool IsActive{get; private set;}

    protected virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = false;        
    }

    public virtual void ActivateWall()
    {
        if (IsActive) return;
        IsActive = true;
        Debug.Log($"{_wallType} Активация стены");
        OnWallActived?.Invoke();
    }
    public virtual void DeactivateWall()
    {
        if (!IsActive) return;
        IsActive = false;
        Debug.Log($"{_wallType} Деактивация стены");
        OnWallDeactived?.Invoke();
    }    

    //типы:
    //подъём/спуск, 
    // уничтожающиеся, уничтожающиеся после прыжка с них, 
    // появляющиеся при приближении(доп колайдер больше чем сама стена) и исчезающая при выходе из поля
    //с аниматором или движение через код
    //уничтожающие персонажа
    //переворачивающие персонажа

    
}
