using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPowerHandler : MonoBehaviour
{
    [SerializeField] private GameEvent_PlayerType _ballBounceEvent;
    [SerializeField] private GameEvent _ballPassMiddle;

    [SerializeField] private bool _testPowerUp = false;
    [SerializeField] private List<PowerUpContainer> _allPowerUps = new List<PowerUpContainer>();
    
    [Header("Offense")]
    [SerializeField] private PowerUpClass _activeOffense;
    [SerializeField] private List<PowerUpClass> _passiveOffenseList = new List<PowerUpClass>();
    
    [Header("Defense")]
    [SerializeField] private PowerUpClass _activeDefense;
    [SerializeField] private List<PowerUpClass> _passiveDefenseList = new List<PowerUpClass>();

    public Action OnBallBounceFromPlayer;
    public Action OnBallBounceFromEnemy;
    public Action OnBallPassMiddle;

    private PlayerMovement _player;

    private void Awake()
    {
        _player = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        if (_testPowerUp)
        {
            PowerUpClass spawnedPower =
                Instantiate(_allPowerUps[0].PowerUp, transform.position, quaternion.identity, transform);
            _activeOffense = spawnedPower;
        }
    }

    private void Update()
    {
        if(_activeOffense != null)
            _activeOffense.OnUpdate(Time.deltaTime);
    }

    private void OnEnable()
    {
        _ballBounceEvent.AddListener(OnBallBounce);
        _ballPassMiddle.AddListener(OnBallPassMiddleFunc);
    }

    private void OnDisable()
    {
        _ballBounceEvent.RemoveListener(OnBallBounce);
        _ballPassMiddle.RemoveListener(OnBallPassMiddleFunc);
    }

    private void OnBallBounce(PlayerType bounceFrom)
    {
        if (bounceFrom == _player.PlayerType1)
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
        if (!_activeOffense.TryToActivate(_player.PlayerType1, this)) return;
        
        _activeOffense.Activate(_player.PlayerType1);
    }

    public void Skill2(InputAction.CallbackContext context)
    {
        //Defensive skill
        if (context.phase != InputActionPhase.Started) return;
        if (_activeDefense == null) return;
        if (!_activeDefense.TryToActivate(_player.PlayerType1, this)) return;
    }
}

[System.Serializable]
public class PowerUpContainer
{
    public PowerCard PowerCard;
    public PowerUpClass PowerUp;
}
