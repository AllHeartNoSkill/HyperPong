using System.Collections.Generic;
using UnityEngine;

public class UIRoot : Singleton<UIRoot>
{
    [SerializeField] private UIStack _menuStack;
    [SerializeField] private UIStack _popupStack;

    private UIInputManager _uiInputManager;
    
    protected override void Awake()
    {
        base.Awake();

        _uiInputManager = GetComponent<UIInputManager>();
    }

    public void OpenMenu(Menu menu)
    {
        _menuStack.DoPush(menu);
    }

    public UIInputManager GetUIInputManager()
    {
        return _uiInputManager;
    }
}
