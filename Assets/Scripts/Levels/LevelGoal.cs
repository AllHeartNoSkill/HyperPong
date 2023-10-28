using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    [SerializeField] private PlayerType _goalOwnedBy;
    [SerializeField] private GameEvent_PlayerType _roundEndEvent;
    
    public void BallTouchGoal()
    {
        _roundEndEvent.TriggerEvent(_goalOwnedBy);
    }
}
