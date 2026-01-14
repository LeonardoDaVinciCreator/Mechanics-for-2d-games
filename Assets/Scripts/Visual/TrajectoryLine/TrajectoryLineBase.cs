using System;
using UnityEngine;

/* 

думаю что для радиусов лучше использовать публичный сетер и выставлять через контреллеры

градиента наверно не будет
 */

public abstract class TrajectoryLineBase : MonoBehaviour
{
    [Header("Center")]
    [SerializeField] 
    protected Transform _centerSlingshot;

    [Header("Line Renderer")]
    [SerializeField] 
    protected LineRenderer _trajectoryLine;

    protected SlingshotControllerBase _slingshotController;
    public SlingshotControllerBase SlingshotController
    {
        set => _slingshotController = value;
    }

    [Header("Circles")]
    [SerializeField] 
    protected LineRenderer _minCircle;

    [SerializeField]
    protected float _minRadius;
    public float MinRadius
    {
        get => _minRadius;
        set {_minRadius = value; UpdateCircles();}
    }        
    
    [SerializeField] 
    protected LineRenderer _maxCircle;
    public float MaxRadius
    {
        get => _maxRadius;
        set {_maxRadius = value; UpdateCircles();}
    }    

    [SerializeField]
    protected float _maxRadius;

    [Header("Settings")]
    [SerializeField] 
    protected int _pointsCount = 30;
    
    [Header("Colors")]
    [SerializeField] 
    protected Color _minColor = Color.green;
    [SerializeField] 
    protected Color _normalColor = Color.yellow;
    [SerializeField] 
    protected Color _maxColor = Color.red;

    

    void Awake()
    {
        SetupVisuals();        
    }    

    private void SetupVisuals()
    {
        SetupLineRenderer(_trajectoryLine, false);
        SetupCircle(_minCircle, _minRadius > 0 ? _minRadius : 1f);
        SetupCircle(_maxCircle, _maxRadius > 0 ? _maxRadius : 2f);
        ShowVisuals(false);
    }

    private void SetupLineRenderer(LineRenderer line, bool enabled)
    {
        if(line == null) return;
        line.positionCount = _pointsCount;
        line.useWorldSpace = true;
        line.enabled = enabled;
    }    

    private void SetupCircle(LineRenderer circle, float radius)
    {
        SetupLineRenderer(circle, false);
        UpdateCircle(circle, radius);
    }

    private void UpdateCircle(LineRenderer circle, float radius)
    {
        if (circle == null) return;

        for (int i = 0; i < _pointsCount; i++)
        {
            float angle = i * Mathf.PI * 2f / _pointsCount;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            circle.SetPosition(i, _centerSlingshot.position + (Vector3)offset);
        }
    }

    protected void UpdateCircles()
    {
        UpdateCircle(_minCircle, _minRadius);
        UpdateCircle(_maxCircle, _maxRadius);
        
        // Цвета для кругов
        if (_minCircle != null) _minCircle.startColor = _minColor;
        if (_maxCircle != null) _maxCircle.startColor = _maxColor;
    }

    protected abstract void UpdateTrajectoryLine();

    public virtual void UpdateAllVisuals()
    {
        UpdateCircles();
        UpdateTrajectoryLine();
    }

    public virtual void ShowVisuals(bool show)
    {
        _trajectoryLine.enabled = show;
        _minCircle.enabled = show;
        _maxCircle.enabled = show;
    }        
}
