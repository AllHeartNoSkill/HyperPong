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
        MoveBall(ballDirection);
    }

    void MoveBall(Vector3 direction){
        // determine speed modifier from direction
        float speedModifier = 1f;

        Vector3 temp = ballTransform.position;
        temp += baseSpeed * speedModifier * direction * Time.deltaTime;

        gameObject.transform.position = temp;
    }

    private void OnCollisionEnter(Collision other) {
        ContactPoint[] contacts = other.contacts;
        // int i = 0;
        // foreach(ContactPoint contact in contacts){
        //     i++;
        //     Debug.Log("hit " + i + contact.normal);
        // }

        ballDirection = Vector3.Reflect(ballDirection, contacts[0].normal);

    }
}
