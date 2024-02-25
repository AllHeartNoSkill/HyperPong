using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BallMovement : MonoBehaviour
{
    [Header("Game Events")]
    [SerializeField] private GameEvent_PlayerType _ballBounceEvent;
    [SerializeField] private GameEvent _ballPassMiddleEvent;
    
    [SerializeField] private GameEvent _ballWallBounce;

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

    private Rigidbody _rb;

    private List<GameObject> _ignoredRaycastObj = new List<GameObject>(); 
    
    public bool DestroyOnMiddle { get; set; }

    public PlayerType InWhatArea => _inWhatArea;

    public float BaseSpeed => baseSpeed;
    
    public SpriteRenderer Sprite { get; private set; }

    private void Awake()
    {
        _ballTransform = gameObject.transform;
        _currentPosition = _ballTransform.position;
        _rb = GetComponent<Rigidbody>();
        Sprite = GetComponentInChildren<SpriteRenderer>();
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBall();
        }
    }

    private void FixedUpdate()
    {
        _lastFramePosition = _currentPosition;
        MoveBall();
        _currentPosition = _ballTransform.position;
    }

    [ContextMenu("Reset Ball")]
    private void ResetBall()
    {
        _ballTransform.position = new Vector3(0f, 0, 0);
        ballDirection = new Vector3(1, 1, 0);
        _currentPosition = _ballTransform.position;
    }

    void MoveBall(){
        // determine speed modifier from direction
        _rb.velocity = ballDirection * baseSpeed;
    }

    private void OnCollisionEnter(Collision other)
    {
        var collisionContact = other.GetContact(0);
        List<ContactPoint> contacts = new List<ContactPoint>();
        other.GetContacts(contacts);
        if (other.collider.TryGetComponent(out LevelGoal goalPost))
        {
            goalPost.BallTouchGoal();
            baseSpeed = Mathf.Clamp(baseSpeed - (_speedIncrement * _roundRestartIncrementMul), _minBallSpeed, _maxBallSpeed);
            gameObject.SetActive(false);
        }
        else if (other.collider.TryGetComponent(out PlayerMovement player))
        {
            ballDirection = player.GetDirectionRelativeToPlayer(collisionContact.point);
            _owner = player.PlayerType;
            _ballBounceEvent.TriggerEvent(_owner);

            baseSpeed = Mathf.Clamp(baseSpeed + _speedIncrement, _minBallSpeed, _maxBallSpeed);
        }
        else
        {
            _ballWallBounce.TriggerEvent();
            ballDirection = Vector3.Reflect(ballDirection, collisionContact.normal).normalized;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerOneArea"))
        {
            if (_inWhatArea != PlayerType.PlayerOne)
            {
                SwitchArea();
            }
        }
        else if (other.CompareTag("PlayerTwoArea"))
        {
            if (_inWhatArea != PlayerType.PlayerTwo)
            {
                SwitchArea();
            }
        } 
    }

    private void SwitchArea()
    {
        _inWhatArea = _inWhatArea == PlayerType.PlayerOne ? PlayerType.PlayerTwo : PlayerType.PlayerOne;
        
        if (DestroyOnMiddle)
        {
            Destroy(gameObject);
        }
        _ballPassMiddleEvent.TriggerEvent();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_lastFramePosition, _currentPosition);
    }

    public void ReverseDirection()
    {
        ballDirection *= -1f;
    }
}
