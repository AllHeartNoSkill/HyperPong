using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventTwoParams<T1, T2> : ScriptableObject
{
    private Action<T1, T2> _onTrigger;
    public void AddListener(Action<T1, T2> listener) => _onTrigger += listener;
    public void RemoveListener(Action<T1, T2> listener) => _onTrigger -= listener;
    public void TriggerEvent(T1 param1, T2 param2) => _onTrigger?.Invoke(param1, param2);
}
