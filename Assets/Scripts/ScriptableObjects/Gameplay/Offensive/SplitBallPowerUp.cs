using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBallPowerUp : PowerUpClass
{
    [Header("Passive")]
    [SerializeField] private float _passiveDuration = 0.3f;
    
    private bool _isPassiveOnGoing = false;
    private float _passiveCountdown;
    private PlayerType _playerType;
    
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
        Debug.Log("Try To Activate Split PU");
        if (!base.TryToActivate(player, playerPowerHandler)) return false;
        return true;
    }

    public override void Activate(PlayerType player)
    {
        Debug.Log("Done Queueing Split PU");
        _playerType = player;
        _playerPowerHandler.OnBallBounceFromPlayer += EffectActivated;
    }

    protected override void EffectActivated()
    {
        base.EffectActivated();
        
        _playerPowerHandler.OnBallBounceFromPlayer -= EffectActivated;

        LevelLoadedData.BallSpawner.SpawnDecoyBall(2, _playerType);
        Debug.Log("ACTIVATE BALL SPLIT");
    }

    protected override void EffectDone()
    {
        base.EffectDone();
    }

    public override void PassiveOnBounceFromPlayer()
    {
        if(_isPassiveOnGoing) return;
        LevelLoadedData.BallSpawner.SpawnDecoyBall(1, _playerType, false);
        _passiveCountdown = 0;
        _isPassiveOnGoing = true;
    }

    protected override void PassiveDone()
    {
        _isPassiveOnGoing = false;
        LevelLoadedData.BallSpawner.DestroyDecoyBalls();
    }
}