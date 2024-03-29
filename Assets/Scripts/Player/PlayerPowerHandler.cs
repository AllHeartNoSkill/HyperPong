using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerPowerHandler : MonoBehaviour
{
    [Header("Game Events")]
    [SerializeField] private GameEvent_PlayerType_PowerCard _skillActiveSelectedEvent;
    [SerializeField] private GameEvent_PlayerType_PowerCard _skillPassiveSelectedEvent;
    [SerializeField] private GameEvent_PlayerType _ballBounceEvent;
    [SerializeField] private GameEvent _ballPassMiddleEvent;

    [Header("Test Param")]
    [SerializeField] private bool _testPowerUp = false;
    [SerializeField] private PowerCard _testActivePower;
    [SerializeField] private List<PowerCard> _testPassivePowers = new List<PowerCard>();
    
    [Header("Offense")]
    [SerializeField] private PowerUpClass _activeOffense;
    [SerializeField] private List<PowerUpClass> _passiveOffenseList = new List<PowerUpClass>();
    
    [Header("Defense")]
    [SerializeField] private PowerUpClass _activeDefense;
    [SerializeField] private List<PowerUpClass> _passiveDefenseList = new List<PowerUpClass>();
    
    private List<PowerUpClass> _allPowerUps = new List<PowerUpClass>();
    private List<PowerCard> _allPowerCards = new List<PowerCard>();
    private List<PowerUpClass> _allPassivesList = new List<PowerUpClass>();

    public Action OnBallBounceFromPlayer;
    public Action OnBallBounceFromEnemy;
    public Action OnBallPassMiddle;

    private PlayerMovement _player;
    private Transform _spawnedSkillContainer;

    public List<PowerCard> AllPowerCards => _allPowerCards;

    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        _spawnedSkillContainer = new GameObject("Skill Container").transform;
        _spawnedSkillContainer.parent = transform;
        
        if (_testPowerUp)
        {
            if(_testActivePower != null) OnSkillActiveSelected(_player.PlayerType, _testActivePower);
            foreach (PowerCard powerCard in _testPassivePowers)
            {
                OnSkillPassiveSelected(_player.PlayerType, powerCard);
            }
        }
    }

    private void Update()
    {
        foreach (var powerUp in _allPowerUps)
        {
            powerUp.OnUpdate(Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        _ballBounceEvent.AddListener(OnBallBounce);
        _ballPassMiddleEvent.AddListener(OnBallPassMiddleFunc);
        _skillActiveSelectedEvent.AddListener(OnSkillActiveSelected);
        _skillPassiveSelectedEvent.AddListener(OnSkillPassiveSelected);
    }

    private void OnDisable()
    {
        _ballBounceEvent.RemoveListener(OnBallBounce);
        _ballPassMiddleEvent.RemoveListener(OnBallPassMiddleFunc);
        _skillActiveSelectedEvent.RemoveListener(OnSkillActiveSelected);
        _skillPassiveSelectedEvent.RemoveListener(OnSkillPassiveSelected);
        
        UnsubscribeAllPassive();
        void UnsubscribeAllPassive()
        {
            foreach (var passivePower in _allPassivesList)
            {
                OnBallBounceFromPlayer -= passivePower.PassiveOnBounceFromPlayer;
                OnBallBounceFromEnemy -= passivePower.PassiveOnBounceFromEnemy;
                OnBallPassMiddle -= passivePower.PassiveOnPassMiddle;
            }
        }
    }

    private void PassiveAddListener(PowerUpClass powerUpClass)
    {
        _allPassivesList.Add(powerUpClass);
        
        OnBallBounceFromPlayer += powerUpClass.PassiveOnBounceFromPlayer;
        OnBallBounceFromEnemy += powerUpClass.PassiveOnBounceFromEnemy;
        OnBallPassMiddle += powerUpClass.PassiveOnPassMiddle;
        powerUpClass.PassiveModifier(this, _player.PlayerType);
    }
    
    private void OnSkillPassiveSelected(PlayerType playerType, PowerCard powerCard)
    {
        if(playerType != _player.PlayerType) return;
        
        PowerUpClass spawnedPower =
            Instantiate(powerCard.PowerUpPrefab, transform.position, quaternion.identity, _spawnedSkillContainer);
        
        _allPowerUps.Add(spawnedPower);
        _allPowerCards.Add(powerCard);
        
        PassiveAddListener(spawnedPower);
    }

    private void OnSkillActiveSelected(PlayerType playerType, PowerCard powerCard)
    {
        if(playerType != _player.PlayerType) return;
        
        PowerUpClass spawnedPower =
            Instantiate(powerCard.PowerUpPrefab, transform.position, quaternion.identity, _spawnedSkillContainer);
        
        _allPowerUps.Add(spawnedPower);
        _allPowerCards.Add(powerCard);

        if (powerCard.type == 0)
        {
            SwitchActiveOffense(spawnedPower);
        }
        else if (powerCard.type == 1)
        {
            SwitchActiveDefense(spawnedPower);
        }
    }

    private void SwitchActiveOffense(PowerUpClass powerUpClass)
    {
        if (_activeOffense != null)
        {
            PassiveAddListener(_activeOffense);
        }

        _activeOffense = powerUpClass;
    }
    
    private void SwitchActiveDefense(PowerUpClass powerUpClass)
    {
        if (_activeDefense != null)
        {
            PassiveAddListener(_activeDefense);
        }

        _activeDefense = powerUpClass;
    }

    private void OnBallBounce(PlayerType bounceFrom)
    {
        if (bounceFrom == _player.PlayerType)
        {
            BallBounceFromPlayer();
        }
        else
        {
            BallBounceFromEnemy();
        }
    }

    private void OnBallPassMiddleFunc()
    {
        OnBallPassMiddle?.Invoke();
    }

    public void BallBounceFromPlayer()
    {
        OnBallBounceFromPlayer?.Invoke();
    }

    public void BallBounceFromEnemy()
    {
        OnBallBounceFromEnemy?.Invoke();
    }

    public void Skill1(InputAction.CallbackContext context)
    {
        //Offensive skill
        if (context.phase != InputActionPhase.Started) return;
        if (_activeOffense == null) return;
        if (!_activeOffense.TryToActivate(_player.PlayerType, this)) return;
        
        _activeOffense.Activate(_player.PlayerType);
    }

    public void Skill2(InputAction.CallbackContext context)
    {
        //Defensive skill
        if (context.phase != InputActionPhase.Started) return;
        if (_activeDefense == null) return;
        if (!_activeDefense.TryToActivate(_player.PlayerType, this)) return;
        
        _activeDefense.Activate(_player.PlayerType);
    }
}

[System.Serializable]
public class PowerUpContainer
{
    public PowerCard PowerCard;
    public PowerUpClass PowerUp;
}
