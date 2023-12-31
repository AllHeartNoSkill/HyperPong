using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Game Events/Create Game Event", fileName = "New Game Event")]
public class GameEvent : ScriptableObject
{
    private Action _onTrigger;
    public void AddListener(Action listener) => _onTrigger += listener;
    public void RemoveListener(Action listener) => _onTrigger -= listener;
    public void TriggerEvent() => _onTrigger?.Invoke();
}
