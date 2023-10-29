using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleBallPowerUp : PowerUpClass
{
    public override void OnUpdate(float deltaTime)
    {
        base.OnUpdate(deltaTime);
    }

    public override bool TryToActivate(PlayerType player, PlayerPowerHandler playerPowerHandler)
    {
        Debug.Log("Try To Activate Invisible PU");
        if (!base.TryToActivate(player, playerPowerHandler)) return false;
        Debug.Log("Success Queueing Invisible PU");
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
        Debug.Log("BALL INVISIBLE DONE");
    }

    public override void Passive(PlayerType player)
    {
        base.Passive(player);
    }
}
