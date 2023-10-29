using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private LevelLoadedData _levelLoadedData;
    
    [SerializeField] private float _randomAngle = 60f;
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private Transform _ballSpawnPoint;

    [Header("Ball Data")]
    [SerializeField] private float _minBallSpeed = 10;
    [SerializeField] private float _maxBallSpeed = 25;
    [SerializeField] private float _speedIncrement = 0.2f;
    [SerializeField] private float _roundRestartIncrementMul = 3f;
    
    [Header("Game Events")]
    [SerializeField] private GameEvent_Int _roundStartEvent;
    [SerializeField] private GameEvent_PlayerType _roundEndEvent;

    private bool _isFirstRound = true;
    private float _ballXDir;
    private BallMovement _spawnedBall;
    private PlayerType _lastRoundWinner;

    private void OnEnable()
    {
        _roundStartEvent.AddListener(OnRoundStart);
        _roundEndEvent.AddListener(OnRoundEnd);
    }

    private void OnDisable()
    {
        _roundStartEvent.RemoveListener(OnRoundStart);
        _roundEndEvent.RemoveListener(OnRoundEnd);
    }

    private void OnRoundEnd(PlayerType winner)
    {
        _lastRoundWinner = winner;
    }

    private void OnRoundStart(int round)
    {
        SpawnBall();
    }

    private void SpawnBall()
    {
        if (_isFirstRound)
        {
            _ballXDir = Random.Range(0, 2) == 0 ? 1f : -1f;
            _spawnedBall = Instantiate(_ballPrefab, _ballSpawnPoint.position, Quaternion.identity).GetComponent<BallMovement>();
            _spawnedBall.SetData(_minBallSpeed, _maxBallSpeed, _speedIncrement, _roundRestartIncrementMul);
            
            _isFirstRound = false;
            _levelLoadedData.SpawnedBall = _spawnedBall;
        }
        else
        {
            _ballXDir = _lastRoundWinner == PlayerType.PlayerOne ? -1f : 1f;
            _spawnedBall.transform.SetPositionAndRotation(_ballSpawnPoint.position, Quaternion.identity);
            _spawnedBall.gameObject.SetActive(true);
        }

        float choosenAngle = Random.Range(90 - (_randomAngle / 2), 90 + (_randomAngle / 2)) * _ballXDir;
        Vector3 ballDirection = Quaternion.AngleAxis(choosenAngle, Vector3.forward) * Vector3.up;
        // Debug.Log($"angle: {choosenAngle} == ball direction: {ballDirection}");
        _spawnedBall.Init(ballDirection, _lastRoundWinner);
    }
}
