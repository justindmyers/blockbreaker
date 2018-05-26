using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour {

    public int lives = 3;
    public Text scoreText;
    public Text livesText;
    private int score = 0;

    private BallController ball;
    private PaddleController paddle;

    // Use this for initialization
    void Start () {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;

        ball = GameObject.FindObjectOfType<BallController>();
        paddle = GameObject.FindObjectOfType<PaddleController>();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable () {
    
    }

    // Update is called once per frame
    void Update () {

    }

    public void onStartClick (string name) {
        SceneManager.LoadScene (name);
    }

    public void onQuitClick () {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }

    public void GoToNextLevel () {
        Debug.Log ("Some Function was called!");
    }

    public void LifeLost() {
        lives -= 1;
        livesText.text = "Lives: " + lives;

        ball.OnResetBall();
    }

    public void BlockDestroyed(Block block) {
        score += block.points;
        scoreText.text = "Score: " + score;

        /*
        // generate random powerup
        GameObject powerUpGameObject = new GameObject("PowerUp");

        //Add Components
        PowerUpController powerUp = powerUpGameObject.AddComponent<PowerUpController>();

        powerUp.Init(PowerUpType.Speed, 5.0f);
         */
    }
}