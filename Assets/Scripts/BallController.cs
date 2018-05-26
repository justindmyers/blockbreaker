using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
    public GameObject ballDestroyedAnimation;

    private PaddleController paddle;
    Rigidbody2D rigidBody;
    SpriteRenderer sprite;

    float m_Speed;

    private Vector3 paddleToBallVector;
    private bool ballActive = false;

    void Start () {
        paddle = GameObject.FindObjectOfType<PaddleController>();
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        ResetBall();
    }

    void Update () {
        if (!ballActive) 
        {
            this.transform.position = paddle.transform.position + paddleToBallVector;

            if (Input.GetMouseButtonDown(0)) 
            {
                m_Speed = 7.0f;

                rigidBody.velocity = Vector2.up * m_Speed;
                rigidBody.AddForce((new Vector2(20f, 20f)) * m_Speed);

                ballActive = true;
            }
        }
    }

    public void SpeedUp() {
        rigidBody.velocity *= 1.01f;
    }

    public void OnResetBall() {
        rigidBody.velocity = Vector2.zero;
        Instantiate(ballDestroyedAnimation, this.transform.position, Quaternion.identity);

        StartCoroutine("waiter");
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(0.5f);
        ResetBall();
    }

    private void ResetBall() {
        this.transform.position = new Vector3(this.transform.position.x - (this.transform.position.x - paddle.transform.position.x), this.transform.position.y - (this.transform.position.y - paddle.transform.position.y) + (sprite.size.y * 3), this.transform.position.z);
        paddleToBallVector = this.transform.position - paddle.transform.position;
        ballActive = false;
    }

}