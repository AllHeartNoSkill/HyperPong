using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Data")]
    [SerializeField] private float _speed = 5;
    [SerializeField] private bool _invertMove = false;
    
    [Header("Path Follower")]
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private EndOfPathInstruction _endOfPathInstruction;

    private float _moveAxis;
    private float _maxDistance;
    private float _currentDistance;

    private void Start()
    {
        if (_pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            _pathCreator.pathUpdated += OnPathChanged;
            _maxDistance = _pathCreator.path.length;
            _currentDistance = _maxDistance / 2;
            transform.position = _pathCreator.path.GetPointAtDistance(_currentDistance, _endOfPathInstruction);
        }
    }

    private void Update()
    {
        if (_moveAxis != 0)
        {
            SetDistance();
            TraversePath();
        }
    }

    public void MoveInput(float axis)
    {
        _moveAxis = axis * (_invertMove ? -1 : 1);
    }

    private void TraversePath()
    {
        transform.position = _pathCreator.path.GetPointAtDistance(_currentDistance, _endOfPathInstruction);
        transform.rotation = _pathCreator.path.GetRotationAtDistance(_currentDistance, _endOfPathInstruction);
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
}
