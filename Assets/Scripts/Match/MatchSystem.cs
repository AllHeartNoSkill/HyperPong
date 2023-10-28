using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSystem : MonoBehaviour
{
    [SerializeField] private bool _playOnAwake = true;
    
    [Header("Game Events")] 
    [SerializeField] private GameEvent_Int _startRoundEvent;
    [SerializeField] private GameEvent_PlayerType _playerScoreAGoalEventPlayerType;

    private void Start()
    {
        if (_playOnAwake)
        {
            
        }
    }

    public void StartGame()
    {
        
    }
}
