using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUpClass : MonoBehaviour
{
    public PowerCard Card;
    public bool RequireOnOwnArea;
    public bool RequireOnEnemyArea;
    public LevelLoadedData LevelLoadedData;

    protected bool IsCooldown = false;
    protected bool IsWaitingToActivated = false;
    protected bool IsEffectActive = false;
    protected PlayerPowerHandler _playerPowerHandler;
    private float _cooldownCounter = 0;
    
    public virtual void OnUpdate(float deltaTime)
    {
        if (IsCooldown)
        {
            _cooldownCounter += deltaTime;
            if (_cooldownCounter >= Card.chargeTime)
            {
                ResetData();
            }
        }
    }

    public virtual void ResetData()
    {
        _cooldownCounter = 0f;
        IsCooldown = false;
    }

    public virtual bool TryToActivate(PlayerType player, PlayerPowerHandler playerPowerHandler)
    {
        if (!InAreaRequirement(player)) return false;
        if (IsCooldown) return false;
        if (IsWaitingToActivated) return false;
        if (IsEffectActive) return false;

        _playerPowerHandler = playerPowerHandler;
        IsWaitingToActivated = true;
        return true;
    }

    public virtual void Activate(PlayerType player) { }

    protected virtual void EffectActivated()
    {
        IsCooldown = true;
        IsWaitingToActivated = false;
        IsEffectActive = true;
    }

    protected virtual void EffectDone()
    {
        IsEffectActive = false;
    }
    
    protected bool InAreaRequirement(PlayerType player)
    {
        if (!RequireOnOwnArea && !RequireOnEnemyArea) return true;
        if (RequireOnOwnArea && LevelLoadedData.SpawnedBall.InWhatArea == player) return true;
        if (RequireOnEnemyArea && LevelLoadedData.SpawnedBall.InWhatArea != player) return true;
        return false;
    }

    public virtual void PassiveOnBounceFromPlayer()
    { }

    public virtual void PassiveOnBounceFromEnemy()
    { }

    public virtual void PassiveOnPassMiddle()
    { }

    public virtual void PassiveModifier(PlayerPowerHandler playerPowerHandler, PlayerType playerType)
    { }
    
    protected virtual void PassiveDone()
    { }
}
