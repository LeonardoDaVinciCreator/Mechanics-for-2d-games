using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuHandlerUIToolkit : MonoBehaviour
{
    private UIDocument _uiDoc;
    private VisualElement _root;
    private VisualElement _panel;



    private void OnEnable()
    {
        _uiDoc = GetComponent<UIDocument>();
        _root = _uiDoc.rootVisualElement;

        //поиск элемента по классу внутри VisualElement
        _panel = _root.Q<VisualElement>("panel");//Q- первый элемент в последовательности
        if(_panel == null) return;

        List<Button> buttons = _panel.Query<Button>(className: "menu-button").ToList();//Query-все элементы в последовательности

        Label panelLabel = _panel.Q<Label>(className:"label");
        if(panelLabel != null)
        {
            panelLabel.text = "Нажми на кнопку";
        }

        foreach(var btn in buttons)
        {
            btn.clicked += () =>
            {
                Debug.Log($"Нажата кнопка: {btn.name}");
                UpdateLabel($"Нажата кнопка: {btn.name}", panelLabel);
            };
        }

        
    }

    private void UpdateLabel(string newText, Label label)
    {        
        label.text = newText;
    }

    private void Osable()
    {
        if(_panel == null) return;

        var buttons = _panel.Query<Button>(className:"menu-button").ToList();
        foreach(var btn in buttons)
        {
            btn.clicked -= null;
        }
    }
}
