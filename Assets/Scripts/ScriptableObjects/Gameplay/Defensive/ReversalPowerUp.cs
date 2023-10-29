using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversalPowerUp : PowerUpClass
{
    private bool _isPassiveOnGoing = false;
    private float _passiveDuration = 0.3f;
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
        Debug.Log("Try To Activate Reversal PU");
        if (!base.TryToActivate(player, playerPowerHandler)) return false;
        return true;
    }

    public override void Activate(PlayerType player)
    {
        Debug.Log("Done Queueing Reversal PU");
        _playerPowerHandler.OnBallBounceFromEnemy += EffectActivated;
    }

    protected override void EffectActivated()
    {
        base.EffectActivated();
        
        _playerPowerHandler.OnBallBounceFromEnemy -= EffectActivated;
        _playerPowerHandler.OnBallPassMiddle += EffectDone;

        Debug.Log("ACTIVATE BALL REVERSE");
    }

    protected override void EffectDone()
    {
        base.EffectDone();
        
        LevelLoadedData.SpawnedBall.ReverseDirection();
        _playerPowerHandler.OnBallPassMiddle -= EffectDone;
    }

    public override void PassiveOnBounceFromPlayer()
    {
    //     if(_isPassiveOnGoing) return;
    //     LevelLoadedData.SpawnedBall.GetComponent<SpriteRenderer>().enabled = false;
    //     _passiveCountdown = 0;
    //     _isPassiveOnGoing = true;
    }

    private void PassiveDone()
    {
    //     _isPassiveOnGoing = false;
    //     LevelLoadedData.SpawnedBall.GetComponent<SpriteRenderer>().enabled = true;
    }
}
