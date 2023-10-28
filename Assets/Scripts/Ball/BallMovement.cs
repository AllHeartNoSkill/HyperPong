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
    private Vector3 _lastFramePosition;
    private GameObject _lastCollidedObject;

    private void Awake()
    {
        ballTransform = gameObject.transform;
    }

    public void Init(Vector3 startDirection)
    {
        ballDirection = startDirection;
    }

    void Update()
    {
        _lastFramePosition = ballTransform.position;
        MoveBall();
        CheckForCollision(ballTransform.position);
        CheckForCollision(_lastFramePosition);
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBall();
        }
    }

    [ContextMenu("Reset Ball")]
    private void ResetBall()
    {
        ballTransform.position = new Vector3(0f, 0, 0);
        ballDirection = new Vector3(1, 0, 0);
    }

    void MoveBall(){
        // determine speed modifier from direction
        ballTransform.position += baseSpeed  * Time.deltaTime * ballDirection;
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
        
        if (hit.transform.TryGetComponent(out PlayerMovement player))
        {
            ballDirection = player.GetDirectionRelativeToPlayer(hit.point);
        }
        else
        {
            ballDirection = Vector3.Reflect(ballDirection, hit.normal).normalized;
        }

        ballDirection.z = 0;
        Debug.Log("hit " + ballDirection);
    }

    private void CheckGoalPost(RaycastHit hit)
    {
        if (hit.transform.TryGetComponent(out LevelGoal goal))
        {
            goal.BallTouchGoal();
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, castRadius);
    }
}
