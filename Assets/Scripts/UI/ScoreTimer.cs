using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreTimer : MonoBehaviour
{
    private UIDocument _uiDoc;
    private VisualElement _root;
    private Label _timerLabel;
    private Label _scoreLabel;
    
    private float _currentTime;
    private int _currentScore;
    private bool _isTimerRunning = true;
    
    [SerializeField] private float _startTime = 300f; // 5 минут
    [SerializeField] private int _startScore = 0;
    
    public event Action OnTimeOut;
    
    private void OnEnable()
    {
        _uiDoc = GetComponent<UIDocument>();
        _root = _uiDoc.rootVisualElement;
        
        _timerLabel = _root.Q<Label>("TimerLabel");
        _scoreLabel = _root.Q<Label>("ScoreLabel");
        
        _currentTime = _startTime;
        _currentScore = _startScore;
        
        UpdateTimerDisplay();
        UpdateScoreDisplay();
    }
    
    private void Update()
    {
        if (_isTimerRunning && _currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            
            if (_currentTime <= 0)
            {
                _currentTime = 0;
                _isTimerRunning = false;
                OnTimeOut?.Invoke();
            }
            
            UpdateTimerDisplay();
        }
    }
    
    private void UpdateTimerDisplay()
    {
        if (_timerLabel != null)
        {
            TimeSpan time = TimeSpan.FromSeconds(_currentTime);
            _timerLabel.text = $"{time.Minutes:00}:{time.Seconds:00}";
        }
    }
    
    private void UpdateScoreDisplay()
    {
        if (_scoreLabel != null)
        {
            _scoreLabel.text = _currentScore.ToString();
        }
    }
    
    public void AddScore(int points)
    {
        _currentScore += points;
        UpdateScoreDisplay();
    }
    
    public void ResetTimer()
    {
        _currentTime = _startTime;
        _isTimerRunning = true;
        UpdateTimerDisplay();
    }
    
    public void ResetScore()
    {
        _currentScore = _startScore;
        UpdateScoreDisplay();
    }
    
    public void PauseTimer()
    {
        _isTimerRunning = false;
    }
    
    public void ResumeTimer()
    {
        _isTimerRunning = true;
    }
    
    public int GetCurrentScore()
    {
        return _currentScore;
    }
    
    public float GetCurrentTime()
    {
        return _currentTime;
    }
}