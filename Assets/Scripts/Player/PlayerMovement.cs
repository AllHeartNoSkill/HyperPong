using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header("Datas")]
    [SerializeField] private LevelLoadedData _levelLoadedData;
    
    [Header("Game Events")]
    [SerializeField] private GameEvent _levelLoadedEvent;
    
    [Header("Player Datas")] 
    [SerializeField] private PlayerType _playerType;
    [SerializeField] private float _speed = 5;
    [SerializeField] private bool _invertMove = false;
    [SerializeField] private float _ballAimReduction = 5f;
    
    [Header("Path Follower")]
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private EndOfPathInstruction _endOfPathInstruction;

    private float _moveAxis;
    private float _maxDistance;
    private float _currentDistance;
    private Vector3 _scaleVector = new Vector3(1f, 0.2f, 1f);

    public PlayerType PlayerType1 => _playerType;

    private void Update()
    {
        if (_moveAxis != 0)
        {
            SetDistance();
            TraversePath();
        }
    }

    private void OnLevelLoaded()
    {
        _pathCreator = _playerType == PlayerType.PlayerOne
            ? _levelLoadedData.PlayerOnePath
            : _levelLoadedData.PlayerTwoPath;

        _speed = _levelLoadedData.PlayerMoveSpeed;

        _scaleVector.z = _levelLoadedData.PlayerLength;
        transform.localScale = _scaleVector;
        
        if (_pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            _pathCreator.pathUpdated += OnPathChanged;
            _maxDistance = _pathCreator.path.length;
            _currentDistance = _maxDistance / 2;
            
            TraversePath();
        }
    }

    public void MoveInput(float axis)
    {
        _moveAxis = axis * (_invertMove ? -1 : 1);
    }

    public Vector3 GetDirectionRelativeToPlayer(Vector3 hitPoint)
    {
        Vector3 result = hitPoint - (transform.position + transform.up * _ballAimReduction);
        return result.normalized;
    }

    private void TraversePath()
    {
        if (_pathCreator != null)
        {
            transform.position = _pathCreator.path.GetPointAtDistance(_currentDistance, _endOfPathInstruction);
            transform.rotation = _pathCreator.path.GetRotationAtDistance(_currentDistance, _endOfPathInstruction);
        }
    }

    private void SetDistance()
    {
        _currentDistance += _speed * _moveAxis * Time.deltaTime;
        _currentDistance = Mathf.Clamp(_currentDistance, 0f, _maxDistance);
    }
    
    private void OnPathChanged() 
    {
        _currentDistance = _pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        _maxDistance = _pathCreator.path.length;
    }

    private void OnEnable()
    {
        _levelLoadedEvent.AddListener(OnLevelLoaded);
    }

    private void OnDisable()
    {
        if (_pathCreator != null)
        {
            _pathCreator.pathUpdated -= OnPathChanged;
        }
        _levelLoadedEvent.RemoveListener(OnLevelLoaded);
    }
}
