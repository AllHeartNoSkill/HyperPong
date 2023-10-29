using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ExtensionPowerUp : PowerUpClass
{
    [SerializeField] private GameEvent_PlayerType _extensionStartEvent;
    [SerializeField] private GameEvent_PlayerType _extensionDoneEvent;
    [SerializeField] private GameEvent_PlayerType _extensionPassiveEvent;
    
    private float _effectDuration = 4f;
    private float _effectCountdown;
    private PlayerType _playerType;
    
    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (IsEffectActive)
        {
            _effectCountdown += deltaTime;
            if (_effectCountdown >= _effectDuration)
            {
                EffectDone();
            }
        }
    }

    public override bool TryToActivate(PlayerType player, PlayerPowerHandler playerPowerHandler)
    {
        Debug.Log("Try To Activate Extension PU");
        if (!base.TryToActivate(player, playerPowerHandler)) return false;
        return true;
    }

    public override void Activate(PlayerType player)
    {
        Debug.Log("Done Queueing Extension PU");
        _playerType = player;
        EffectActivated();
    }

    protected override void EffectActivated()
    {
        base.EffectActivated();
        Debug.Log("ACTIVATE EXTENSION");
        _extensionStartEvent.TriggerEvent(_playerType);
    }

    protected override void EffectDone()
    {
        base.EffectDone();
        _effectCountdown = 0f;
        _extensionDoneEvent.TriggerEvent(_playerType);
    }

    public override void PassiveModifier(PlayerPowerHandler playerPowerHandler, PlayerType playerType)
    {
        print("trigger passive extension");
        _extensionPassiveEvent.TriggerEvent(playerType);
    }
}
