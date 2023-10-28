using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{

    [SerializeField] private float baseSpeed = 1f; // shouldn't be here
    [SerializeField] private Vector3 ballDirection = new Vector3(1, 0, 0);
    [SerializeField] private float castRadius = 1f;

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
        CheckForCollision();
        MoveBall();
    }

    [ContextMenu("Reset Ball")]
    private void ResetBall()
    {
        ballTransform.position = new Vector3(0f, 0, 0);
        ballDirection = new Vector3(1, 0, 0);
    }

    void MoveBall(){
        // determine speed modifier from direction
        float speedModifier = 1f;

        ballTransform.position += baseSpeed * speedModifier  * Time.deltaTime * ballDirection;
    }

    void CheckForCollision(){
        RaycastHit hit;

        Vector3 p1 = transform.position;
        float distanceToObstacle = 0;

        if (Physics.SphereCast(p1, castRadius / 2, ballDirection, out hit, 10))
        {
            distanceToObstacle = hit.distance;
            if(distanceToObstacle < castRadius){
                ballDirection = Vector3.Reflect(ballDirection, hit.normal);
                Debug.Log("hit " + ballDirection);

            }
        }
    }
}
