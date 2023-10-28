using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Transform ballTransform;

    [SerializeField] private float baseSpeed = 1f; // shouldn't be here
    [SerializeField] private Vector3 ballDirection = new Vector3(1, 0, 0);
    void Start()
    {
        ballTransform = gameObject.transform;
    }

    void Update()
    {
        MoveBall();

        if(ballTransform.position.y < -10){
            ballTransform.position = new Vector3(-8f, 0, 0);
            ballDirection = new Vector3(1, 0, 0);
        }
    }

    void MoveBall(){
        // determine speed modifier from direction
        float speedModifier = 1f;

        // Vector3 temp = ballTransform.position;
        // temp += 

        ballTransform.position += baseSpeed * speedModifier  * Time.deltaTime * ballDirection;
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Bounce Wall"){
            ContactPoint[] contacts = other.contacts;
            // int i = 0;
            // foreach(ContactPoint contact in contacts){
            //     i++;
            // }

            Debug.Log("hit " + contacts[0].normal);
            ballDirection = Vector3.Reflect(ballDirection, contacts[0].normal);
            Debug.Log("newDie " + ballDirection);

        }

    }
}
