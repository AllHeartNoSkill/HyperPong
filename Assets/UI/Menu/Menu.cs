using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [HideInInspector]
    public UIStack CurrentStack;

    protected UIInputManager _uiInputManager;
    protected Canvas _canvas;

    protected virtual void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();
    }

    public virtual void PrePush()
    {
        _uiInputManager = UIRoot.instance.GetUIInputManager();
    }

    public virtual void Kill()
    {
        CurrentStack.DoPopMenu(this);
    }
}
