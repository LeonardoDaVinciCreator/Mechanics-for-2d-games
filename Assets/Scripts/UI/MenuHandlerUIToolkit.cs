using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;



public class MenuHandlerUIToolkit : MonoBehaviour
{
    private UIDocument _uiDoc;
    private VisualElement _root;
    private VisualElement _pauseMenu;
    private VisualElement _settingsMenu;
    private VisualElement _mainMenu;

    private ScoreTimer _scoreTimer;

    private Action _pauseAction;
    private Action _settingsAction;

    private Button _pauseMenuButton;
    private Button _shopMenuButton;    

    private Button _exitSettingButton;

    [SerializeField] private bool _isPaused;
    [SerializeField] private bool _isSettings;

    private void OnEnable()
    {
        _uiDoc = GetComponent<UIDocument>();
        _root = _uiDoc.rootVisualElement;
        _scoreTimer = GetComponent<ScoreTimer>();

        _pauseAction = ToglePauseMenu;
        _settingsAction = TogleSettingsMenu;

        InitializeUI();
        SetupSliderCallbacks();
        
    }

    private void InitializeUI()
    {
        //поиск элемента по классу внутри VisualElement
        _pauseMenu = _root.Q<VisualElement>("PauseMenu");
        _settingsMenu = _root.Q<VisualElement>("SettingsMenu");
        _shopMenuButton = _root.Q<Button>("ShopButton");
        _pauseMenuButton = _root.Q<Button>("PauseButton");
        
        if (_pauseMenuButton == null || _pauseMenu == null) return;
        _pauseMenuButton.clicked += _pauseAction;

        SetupPauseMenuButtons();
        SetupSettingsMenuButtons();
    }

    private void SetupPauseMenuButtons()
    {
         var pauseMenuCard = _pauseMenu.Q<VisualElement>("PauseMenuCard");
        if (pauseMenuCard == null) return;
        
        List<Button> buttons = pauseMenuCard.Query<Button>(className: "menu-button").ToList();

        foreach (var btn in buttons)
        {
            switch (btn.name)
            {
                case "ResumeButton":
                    btn.clicked += _pauseAction;
                    break;
                case "SettingsButton":
                    btn.clicked += _settingsAction;
                    break;
                case "MainMenuButton":
                    btn.clicked += TogleMainMenu;
                    break;
            }
        }
    }

    private void SetupSettingsMenuButtons()
    {
        if (_settingsMenu == null) return;
        
        var settingsMenuCard = _settingsMenu.Q<VisualElement>("SettingsMenuCard");
        if (settingsMenuCard == null) return;
        
        var applyBtn = settingsMenuCard.Q<Button>("ApplyButton");
        var cancelBtn = settingsMenuCard.Q<Button>("CancelButton");
        
        if (applyBtn != null)
            applyBtn.clicked += _settingsAction;
            
        if (cancelBtn != null)
            cancelBtn.clicked += _settingsAction;
    }

    private void SetupSliderCallbacks()
    {
        if (_settingsMenu == null) return;
        
        SetupSlider("MusicSlider", "MusicValue");
        SetupSlider("SfxSlider", "SfxValue");
        SetupSlider("VibrationSlider", "VibrationValue");
    }

    private void SetupSlider(string sliderName, string valueLabelName)
    {
        var slider = _settingsMenu.Q<Slider>(sliderName);
        var valueLabel = _settingsMenu.Q<Label>(valueLabelName);
        
        if (slider != null && valueLabel != null)
        {
            slider.RegisterValueChangedCallback(evt =>
            {
                valueLabel.text = $"{Mathf.RoundToInt(evt.newValue)}%";
            });
        }
    }

    private void TogleMainMenu()
    {
        Debug.Log("Переход в главное меню");
        Time.timeScale = 1;
    }

    private void TogleSettingsMenu()
    {
        if(_settingsMenu == null) return;        
        _isSettings = !_isSettings;
        if (_isSettings)
        {
            _pauseMenu.RemoveFromClassList("show");
            _settingsMenu.AddToClassList("show");            
        }
        else
        {
            _settingsMenu.RemoveFromClassList("show");
            _pauseMenu.AddToClassList("show");

            //возобновление таймера
            if (_scoreTimer != null && _isPaused)
                _scoreTimer.ResumeTimer();
        }
    }

    private void UpdateLabel(string newText, Label label)
    {        
        label.text = newText;
    }

    private void ToglePauseMenu()
    {
        if(_pauseMenu == null) return;
        _isPaused = !_isPaused;

        if (_isPaused)
        {
            Time.timeScale = 0;
            _pauseMenu.AddToClassList("show");//добавление класса show в css
            _pauseMenuButton.SetEnabled(false);
            _pauseMenuButton.AddToClassList("hide");
            _shopMenuButton.AddToClassList("hide");
            _shopMenuButton.SetEnabled(false);

            if (_scoreTimer != null)
                _scoreTimer.PauseTimer();
        }
        else
        {
            Time.timeScale = 1;
            _pauseMenu.RemoveFromClassList("show");
            _pauseMenuButton.SetEnabled(true);
            _pauseMenuButton.RemoveFromClassList("hide");
            _shopMenuButton.RemoveFromClassList("hide");
            _shopMenuButton.SetEnabled(true);

            if (_isSettings)
            {
                _isSettings = false;
                _settingsMenu.RemoveFromClassList("show");
            }
            if (_scoreTimer != null)
                _scoreTimer.ResumeTimer();
        }
    }

    private void OnDisable()
    {
        if(_pauseMenuButton == null) return;
            _pauseMenuButton.clicked -= _pauseAction;              

        CleanupMenuButtons();
    }

    private void CleanupMenuButtons()
    {
        var allMenus = new[] { _pauseMenu, _settingsMenu };

        foreach(var menu in allMenus)
        {
            if(menu == null) continue;
            var buttons = menu.Query<Button>(className: "menu-button").ToList();
            foreach(var btn in buttons) btn.clicked -= null;
        }
    }
}