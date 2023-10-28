using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{

    [SerializeField] private float baseSpeed = 1f; // shouldn't be here
    [SerializeField] private Vector3 ballDirection = new Vector3(1, 0, 0);
    
    private Transform ballTransform;
    private Rigidbody _rigidBody;
    private GameObject _lastCollidedObject;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        ballTransform = gameObject.transform;
    }

    void Start()
    {
        
    }

    void Update()
    {
        // MoveBall();

    }

    [ContextMenu("Reset Ball")]
    private void ResetBall()
    {
        ballTransform.position = new Vector3(0f, 0, 0);
        ballDirection = new Vector3(1, 0, 0);
    }

    private void FixedUpdate()
    {
        // ballTransform.position += baseSpeed * Time.fixedDeltaTime * ballDirection;
        _rigidBody.velocity = baseSpeed * ballDirection;
    }

    void MoveBall(){
        // determine speed modifier from direction
        float speedModifier = 1f;

        // Vector3 temp = ballTransform.position;
        // temp += 

        ballTransform.position += baseSpeed * speedModifier  * Time.deltaTime * ballDirection;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject == _lastCollidedObject) return;
        _lastCollidedObject = other.gameObject;
        
        if(other.gameObject.tag == "Bounce Wall"){
            ReflectBall(other);
        }
    }

    private void OnCollisionStay(Collision other) {
        if(other.gameObject == _lastCollidedObject) return;
        _lastCollidedObject = other.gameObject;
        
        if(other.gameObject.tag == "Bounce Wall"){
            ReflectBall(other);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        _lastCollidedObject = null;
    }

    private void ReflectBall(Collision other)
    {
        ContactPoint[] contacts = other.contacts;
        
        Debug.Log("hit " + contacts[0].normal);
        ballDirection = Vector3.Reflect(ballDirection, contacts[0].normal);
        ballDirection.z = 0f;
        Debug.Log("newDie " + ballDirection);

    }
}
