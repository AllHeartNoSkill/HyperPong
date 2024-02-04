using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudPitPowerUp : PowerUpClass
{
    [Header("Active")]
    [SerializeField] private float _effectDuration = 5f;
    
    [Header("Game Events")]
    [SerializeField] private GameEvent_PlayerType _mudPitStartEvent;
    [SerializeField] private GameEvent_PlayerType _mudPitDoneEvent;
    [SerializeField] private GameEvent_PlayerType _mudPitPassiveEvent;
    
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
        Debug.Log("Try To Activate Mud Pit PU");
        if (!base.TryToActivate(player, playerPowerHandler)) return false;
        return true;
    }

    public override void Activate(PlayerType player)
    {
        Debug.Log("Done Queueing Mud Pit PU");
        _playerType = player;
        EffectActivated();
    }

    protected override void EffectActivated()
    {
        base.EffectActivated();
        Debug.Log("ACTIVATE MUD PIT");
        _mudPitStartEvent.TriggerEvent(_playerType);
    }

    protected override void EffectDone()
    {
        base.EffectDone();
        _effectCountdown = 0f;
        _mudPitDoneEvent.TriggerEvent(_playerType);
    }

    public override void PassiveModifier(PlayerPowerHandler playerPowerHandler, PlayerType playerType)
    {
        print("trigger passive mud pit");
        _mudPitPassiveEvent.TriggerEvent(playerType);
    }
}
