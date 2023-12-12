using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class AutoTraversePath : MonoBehaviour
{
    [Header("Path Follower")]
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private EndOfPathInstruction _endOfPathInstruction;

    private float _moveAxis;
    private float _maxDistance;
    private float _currentDistance;
    [SerializeField] float speed = 1;
    [SerializeField] float initPosition;

    private void Update() {
        _currentDistance += Time.deltaTime * speed;
        TraversePath();
    }

    private void OnPathChanged() 
    {
        _currentDistance = _pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        _maxDistance = _pathCreator.path.length;
    }

    private void TraversePath()
    {
        if (_pathCreator != null)
        {
            transform.position = _pathCreator.path.GetPointAtDistance(_currentDistance, _endOfPathInstruction);
            transform.rotation = _pathCreator.path.GetRotationAtDistance(_currentDistance, _endOfPathInstruction);
        }
    }

    private void OnEnable()
    {
        
        if (_pathCreator != null)
        {
            // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
            _pathCreator.pathUpdated += OnPathChanged;
            _maxDistance = _pathCreator.path.length;
            _currentDistance = _maxDistance * initPosition;
            
            TraversePath();
        }
    }

    private void OnDisable()
    {
        if (_pathCreator != null)
        {
            _pathCreator.pathUpdated -= OnPathChanged;
        }
    }
}
