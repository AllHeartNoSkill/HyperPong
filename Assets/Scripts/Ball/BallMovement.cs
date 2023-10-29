using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [Header("Game Events")]
    [SerializeField] private GameEvent_PlayerType _ballBounceEvent;
    [SerializeField] private GameEvent _ballPassMiddle;
    
    [SerializeField] private float baseSpeed = 1f; // shouldn't be here
    [SerializeField] private Vector3 ballDirection = new Vector3(1, 0, 0);
    [SerializeField] private float castRadius = 1f;
    
    private float _minBallSpeed;
    private float _maxBallSpeed;
    private float _speedIncrement;
    private float _roundRestartIncrementMul;

    private bool _middleHitRequest = false;
    private Transform _ballTransform;
    private Vector3 _lastFramePosition;
    private GameObject _lastCollidedObject;
    private PlayerType _owner;
    private PlayerType _inWhatArea;

    public PlayerType InWhatArea => _inWhatArea;

    private void Awake()
    {
        _ballTransform = gameObject.transform;
    }

    public void SetData(float minSpeed, float maxSpeed, float speedIncrement, float roundRestartIncrementMul)
    {
        _minBallSpeed = minSpeed;
        _maxBallSpeed = maxSpeed;
        _speedIncrement = speedIncrement;
        _roundRestartIncrementMul = roundRestartIncrementMul;
        baseSpeed = minSpeed;
    }

    public void Init(Vector3 startDirection, PlayerType owner)
    {
        ballDirection = startDirection;
        _owner = owner;
    }

    void Update()
    {
        _lastFramePosition = _ballTransform.position;
        MoveBall();
        CheckForCollision(_ballTransform.position);
        CheckForCollision(_lastFramePosition);
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBall();
        }
    }

    [ContextMenu("Reset Ball")]
    private void ResetBall()
    {
        _ballTransform.position = new Vector3(0f, 0, 0);
        ballDirection = new Vector3(1, 0, 0);
    }

    void MoveBall(){
        // determine speed modifier from direction
        _ballTransform.position += baseSpeed  * Time.deltaTime * ballDirection;
    }

    void CheckForCollision(Vector3 startPosition){
        RaycastHit hit;
        float distanceToObstacle = 0;

        if (Physics.SphereCast(startPosition, castRadius, ballDirection, out hit, 1))
        {
            distanceToObstacle = hit.distance;
            if(distanceToObstacle < castRadius){
                ReflectBall(hit);
            }
        }
    }

    private void ReflectBall(RaycastHit hit)
    {
        CheckGoalPost(hit);
        CheckMiddleArea(hit);
        
        if(hit.collider.isTrigger) return;
        
        if (hit.transform.TryGetComponent(out PlayerMovement player))
        {
            ballDirection = player.GetDirectionRelativeToPlayer(hit.point);
            CollideWithPlayer(player);
        }
        else
        {
            ballDirection = Vector3.Reflect(ballDirection, hit.normal).normalized;
        }

        _middleHitRequest = false;

        ballDirection.z = 0;
        // Debug.Log("hit " + ballDirection);
    }

    private void CollideWithPlayer(PlayerMovement player)
    {
        _owner = player.PlayerType1;
        _inWhatArea = SwitchAreaFrom(_owner);
        _ballBounceEvent.TriggerEvent(_owner);

        baseSpeed += _speedIncrement;
    }

    private PlayerType SwitchAreaFrom(PlayerType currentArea)
    {
        return currentArea == PlayerType.PlayerOne ? PlayerType.PlayerTwo : PlayerType.PlayerOne;
    }

    private void CheckGoalPost(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out LevelGoal goal))
        {
            goal.BallTouchGoal();
            baseSpeed -= (_speedIncrement * _roundRestartIncrementMul);
            gameObject.SetActive(false);
        }
    }

    private void CheckMiddleArea(RaycastHit hit)
    {
        LevelMiddle midPoint;
        if (!hit.transform.TryGetComponent(out midPoint)) return;
        if(_middleHitRequest) return;
        _middleHitRequest = true;
        _inWhatArea = SwitchAreaFrom(_inWhatArea);
        _ballPassMiddle.TriggerEvent();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, castRadius);
    }
}
