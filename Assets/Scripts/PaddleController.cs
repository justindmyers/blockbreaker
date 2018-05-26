using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour {
    public float movementSpeed;

    private BallController ball;

    void Start () {
        ball = GameObject.FindObjectOfType<BallController>();
    }

    // Update is called once per frame
    void Update () {
        Vector3 clampedPosition = transform.position;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            //print();
            //transform.Translate(Time.deltaTime * speed * -1.0f, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            //transform.Translate(Time.deltaTime * speed, 0f, 0f);
        }

        transform.Translate(Time.deltaTime * movementSpeed * Input.GetAxis("Horizontal"), 0f, 0f);

        clampedPosition.x = Mathf.Clamp(transform.position.x, 3.1f, 28.9f);
        transform.position = clampedPosition;
    }

    void OnCollisionEnter2D (Collision2D coll) {
        Vector3 normal = coll.contacts[0].normal;
        Vector3 velocity = coll.rigidbody.velocity;
        Vector2 contact = coll.contacts[0].point;

        //Debug.Log("World Contact: " + coll.contacts[0].point);   

        //Vector3 local_vec = transform.InverseTransformPoint (contact);

        //Debug.Log("Local Contact: " + local_vec + ", Angle: " + Vector3.Angle(velocity, normal));

        //local contact is between -1.1 and 1.1

        double myAngleInRadians = (Math.PI / 180) * (((coll.transform.position.x - coll.otherCollider.transform.position.x) * 60f) + 90f);
        Vector2 angleVector = new Vector2((float) Math.Cos(myAngleInRadians), -(float)Math.Sin(myAngleInRadians));

        Debug.DrawRay(coll.contacts[0].point, angleVector, Color.green, 2, false);

        Debug.DrawRay(coll.contacts[0].point, coll.contacts[0].normal, Color.white, 2, false);

        // draw the normal when it makes contact
        //Debug.DrawLine(coll.contacts[0].point, Vector3.Reflect(coll.rigidbody.velocity, coll.contacts[0].normal), Color.white, 2, false);

        //coll.rigidbody.velocity = Vector2.zero;
        //coll.rigidbody.angularVelocity = 0;

        //coll.rigidbody.transform.Rotate(10f, 0, 0);
        //coll.rigidbody.velocity = Vector2.up * 10.0f;

        //coll.rigidbody.AddForce(angleVector * 10.0f);

        //-(coll.transform.position.x - coll.otherCollider.transform.position.x) * 60f) + 90f;

        //Debug.Log(angleVector);

        ball.SpeedUp();
    }

}