using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleBallPowerUp : PowerUpClass
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
        Debug.Log("Try To Activate Invisible PU");
        if (!base.TryToActivate(player, playerPowerHandler)) return false;
        return true;
    }

    public override void Activate(PlayerType player)
    {
        Debug.Log("Done Queueing Invisible PU");
        _playerPowerHandler.OnBallBounceFromPlayer += EffectActivated;
    }

    protected override void EffectActivated()
    {
        base.EffectActivated();
        
        _playerPowerHandler.OnBallBounceFromPlayer -= EffectActivated;
        _playerPowerHandler.OnBallPassMiddle += EffectDone;

        LevelLoadedData.SpawnedBall.GetComponent<SpriteRenderer>().enabled = false;
        Debug.Log("ACTIVATE BALL INVISIBLE");
    }

    protected override void EffectDone()
    {
        base.EffectDone();
        
        LevelLoadedData.SpawnedBall.GetComponent<SpriteRenderer>().enabled = true;
        _playerPowerHandler.OnBallPassMiddle -= EffectDone;
    }

    public override void PassiveOnBounceFromPlayer()
    {
        if(_isPassiveOnGoing) return;
        LevelLoadedData.SpawnedBall.GetComponent<SpriteRenderer>().enabled = false;
        _passiveCountdown = 0;
        _isPassiveOnGoing = true;
    }

    private void PassiveDone()
    {
        _isPassiveOnGoing = false;
        LevelLoadedData.SpawnedBall.GetComponent<SpriteRenderer>().enabled = true;
    }
}
