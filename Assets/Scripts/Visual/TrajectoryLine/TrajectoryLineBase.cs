using UnityEngine;

public class TrajectoryLineBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject _prefabBird;
    [SerializeField]
    protected GameObject _slingshot;
    [SerializeField]
    protected LineRenderer _trajectoryLine;
    [SerializeField]
    protected string[] _tagsObjects;

    [Space(10)]
    [Header("Settings")]
    [SerializeField]
    protected Color _trajectoryLineColor;

    [SerializeField]
    private float _widthTrajectoryLine;
    [SerializeField]
    private int _positionCount;
    [SerializeField] Vector3[] _positions;

    private int positionCountCurrent;

    private void Start()
    {
        _trajectoryLine.positionCount = _positionCount;
        for(int i = 0; i<_positionCount; i++)
        {
            _trajectoryLine.SetPosition(i, _positions[i]);
        }
        Gradient gradient = new Gradient();
        gradient.colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(_trajectoryLineColor, 0f),
            new GradientColorKey(_trajectoryLineColor, 1f)
        };
        gradient.alphaKeys = new GradientAlphaKey[] 
        { 
            new(1f, 0f), 
            new(1f, 1f) 
        };
        _trajectoryLine.colorGradient = gradient;
        _trajectoryLine.startWidth = _widthTrajectoryLine;
        _trajectoryLine.endWidth = _widthTrajectoryLine;

        positionCountCurrent = _positionCount;
    }

    private void Update()
    {
        if(positionCountCurrent!= _positionCount)
        {
            _trajectoryLine.positionCount = _positionCount;
            for (int i = 0; i < _positionCount; i++)
            {
                _trajectoryLine.SetPosition(i, _positions[i]);
            }
            Gradient gradient = new Gradient();
            gradient.colorKeys = new GradientColorKey[]
            {
            new GradientColorKey(_trajectoryLineColor, 0f),
            new GradientColorKey(_trajectoryLineColor, 1f)
            };
            gradient.alphaKeys = new GradientAlphaKey[]
            {
            new(1f, 0f),
            new(1f, 1f)
            };
            _trajectoryLine.colorGradient = gradient;
            _trajectoryLine.startWidth = _widthTrajectoryLine;
            _trajectoryLine.endWidth = _widthTrajectoryLine;

            positionCountCurrent = _positionCount;
        }
    }
}
