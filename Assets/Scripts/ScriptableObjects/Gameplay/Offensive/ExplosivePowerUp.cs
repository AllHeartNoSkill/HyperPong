using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivePowerUp : PowerUpClass
{
    [Header("Active")]
    [SerializeField] private float _activeSpeedMultiplier = 2f;
    
    [Header("Passive")]
    [SerializeField] private float _passiveDuration = 0.3f;

    [SerializeField] private float _passiveSpeedMultipler = 0.1f;
    
    private bool _isPassiveOnGoing = false;
    private float _passiveCountdown;
    
    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
        if (_isPassiveOnGoing)
        {
            _passiveCountdown += deltaTime;
            if (_passiveCountdown >= _passiveDuration)
            {
                PassiveDone();
            }
        }
    }

    public override bool TryToActivate(PlayerType player, PlayerPowerHandler playerPowerHandler)
    {
        Debug.Log("Try To Activate Explosion PU");
        if (!base.TryToActivate(player, playerPowerHandler)) return false;
        return true;
    }

    public override void Activate(PlayerType player)
    {
        Debug.Log("Done Queueing Explosion PU");
        _playerPowerHandler.OnBallBounceFromPlayer += EffectActivated;
    }

    protected override void EffectActivated()
    {
        base.EffectActivated();
        
        _playerPowerHandler.OnBallBounceFromPlayer -= EffectActivated;
        _playerPowerHandler.OnBallBounceFromPlayer += EffectDone;

        Debug.Log("ACTIVATE EXPLOSION");
    }

    protected override void EffectDone()
    {
        base.EffectDone();
        _playerPowerHandler.OnBallBounceFromPlayer -= EffectDone;
    }

    public override void PassiveOnBounceFromPlayer()
    {
        
    }

    protected override void PassiveDone()
    {
        
    }
}
