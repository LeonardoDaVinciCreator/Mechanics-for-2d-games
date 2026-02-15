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
        _pauseAction = ToglePauseMenu;
        _settingsAction = TogleSettingsMenu;

        //поиск элемента по классу внутри VisualElement
        _pauseMenu = _root.Q<VisualElement>("PauseMenu");//Q- первый элемент в последовательности
        _settingsMenu = _root.Q<VisualElement>("SettingsMenu");
        _shopMenuButton = _root.Q<Button>("ShopButton");
       
        _pauseMenuButton = _root.Q<Button>("PauseButton");
        if(_pauseMenuButton == null || _pauseMenu == null) return;
        _pauseMenuButton.clicked += _pauseAction;

        var pauseLabel = _pauseMenu.Q<Label>("PauseLabel");
        var pauseMenuCard = _pauseMenu.Q<VisualElement>("PauseMenuCard");
        List<Button> buttons = pauseMenuCard.Query<Button>(className: "menu-button").ToList();//Query-все элементы в последовательности     

        Label settingsLabel = _settingsMenu.Q<Label>("SettingsLabel");
        if(_settingsMenu != null)
        {
            var settingsMenuCard = _settingsMenu.Q<VisualElement>("SettingsMenuCard");
            if(settingsMenuCard != null)
            {                
                var applyBtn = settingsMenuCard.Q<Button>("ApplyButton");
                var cancelBtn = settingsMenuCard.Q<Button>("CancelButton");
                
                applyBtn.clicked += _settingsAction;
                cancelBtn.clicked += _settingsAction;
            }
        }
        


        foreach(var btn in buttons)
        {
            switch (btn.name)
            {
                case "ResumeButton":
                {
                    btn.clicked += _pauseAction;
                    break;
                }
                case "SettingsButton":
                {
                    btn.clicked += _settingsAction;
                    break;
                }
                case "MainMenuButton":
                {
                    btn.clicked += TogleMainMenu;
                    break;
                }
                
            }            
        }

        
    }

    private void TogleMainMenu()
    {
        throw new NotImplementedException();
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
        }
        else
        {
            Time.timeScale = 1;
            _pauseMenu.RemoveFromClassList("show");///удаление класса show в css
            _pauseMenuButton.SetEnabled(true);
            _pauseMenuButton.RemoveFromClassList("hide");
            _shopMenuButton.RemoveFromClassList("hide");
            _shopMenuButton.SetEnabled(true);
        }
    }

    private void OnDisable()
    {
        if(_pauseMenuButton == null) return;
        _pauseMenuButton.clicked -= _pauseAction;
        if(_pauseMenu == null || _settingsMenu == null) return;        

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
