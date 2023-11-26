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
    
    [Header("Collision Check")]
    [SerializeField] private float castRadius = 1f;
    [SerializeField] private LayerMask _collisionLayers;
    
    private float _minBallSpeed;
    private float _maxBallSpeed;
    private float _speedIncrement;
    private float _roundRestartIncrementMul;

    private bool _middleHitRequest = false;
    private Transform _ballTransform;
    private Vector3 _lastFramePosition;
    private Vector3 _currentPosition;
    private Collider _lastCollidedObject;
    private PlayerType _owner;
    private PlayerType _inWhatArea;

    private List<GameObject> _ignoredRaycastObj = new List<GameObject>(); 
    
    public bool DestroyOnMiddle { get; set; }

    public PlayerType InWhatArea => _inWhatArea;

    public float BaseSpeed => baseSpeed;

    private void Awake()
    {
        _ballTransform = gameObject.transform;
        _currentPosition = _ballTransform.position;
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
        _currentPosition = _ballTransform.position;
        ballDirection = startDirection;
        _owner = owner;
    }

    void Update()
    {
        _lastFramePosition = _currentPosition;
        MoveBall();
        _currentPosition = _ballTransform.position;
        CheckForCollision(_currentPosition);
        CheckForCollision(_lastFramePosition, Vector3.Distance(_currentPosition, _lastFramePosition), true);
        
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
        _currentPosition = _ballTransform.position;
    }

    void MoveBall(){
        // determine speed modifier from direction
        _ballTransform.position += baseSpeed  * Time.deltaTime * ballDirection;
    }

    void CheckForCollision(Vector3 startPosition, float distance = 1f, bool continuousDetect = false){
        RaycastHit hit;
        float distanceToObstacle = 0;

        if (Physics.SphereCast(startPosition, castRadius, ballDirection, out hit, distance, _collisionLayers))
        {
            distanceToObstacle = hit.distance;
            if(distanceToObstacle < castRadius || continuousDetect){
                Debug.Log($"habib - {hit.transform.name}");
                ReflectBall(hit, continuousDetect);
            }
        }
        
        ClearIgnoredObjects();
    }

    private void ClearIgnoredObjects()
    {
        foreach (var ignoredObj in _ignoredRaycastObj)
        {
            ignoredObj.layer = 0;
        }
        _ignoredRaycastObj.Clear();
    }

    private void ReflectBall(RaycastHit hit, bool continuousDetect = false)
    {
        CheckGoalPost(hit);
        CheckMiddleArea(hit);

        Vector3 correctedNextPosition = hit.point + (hit.normal * castRadius);
        if (hit.collider.isTrigger)
        {
            GameObject colliderObj = hit.collider.gameObject;
            colliderObj.layer = 2;
            _ignoredRaycastObj.Add(colliderObj);
            CheckForCollision(correctedNextPosition, Vector3.Distance(_currentPosition, correctedNextPosition), true);
            return;
        }

        if (continuousDetect)
        {
            _ballTransform.position = correctedNextPosition;
            _currentPosition = _ballTransform.position;
        }
        
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

        // baseSpeed += _speedIncrement;
        baseSpeed = Mathf.Clamp(baseSpeed + _speedIncrement, _minBallSpeed, _maxBallSpeed);
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
            // baseSpeed -= (_speedIncrement * _roundRestartIncrementMul);
            baseSpeed = Mathf.Clamp(baseSpeed - (_speedIncrement * _roundRestartIncrementMul), _minBallSpeed, _maxBallSpeed);
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
        if (DestroyOnMiddle)
        {
            Destroy(gameObject);
        }
        _ballPassMiddle.TriggerEvent();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, castRadius);
        Gizmos.DrawLine(_lastFramePosition, _currentPosition);
    }

    public void ReverseDirection()
    {
        ballDirection = ballDirection * -1f;
    }
}
