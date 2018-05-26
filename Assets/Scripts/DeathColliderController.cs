using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathColliderController : MonoBehaviour {

	// Use this for initialization
	private LevelManager levelManager;

	void Start () {
		levelManager = GameObject.FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
    void OnCollisionEnter2D (Collision2D coll) {
        levelManager.LifeLost();
    }
}
