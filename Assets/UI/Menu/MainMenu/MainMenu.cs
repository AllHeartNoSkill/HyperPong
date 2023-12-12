using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : Menu
{
    [SerializeField] private GameObject _firstSelected;

    public override void PrePush()
    {
        base.PrePush();
        _uiInputManager.SetPlayerOneInput(_canvas.gameObject, _firstSelected);
    }

    public void StartLocalMultiplayer()
    {
        if(SystemRoot.TryGetSystem<MatchSystem>(out MatchSystem matchSystem))
        {
            matchSystem.StartGame(1);
            Kill();
        }
    }
    
    public void OpenCredits()
    {
        
    }
}
