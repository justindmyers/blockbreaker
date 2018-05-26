using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType { Speed, Power };

public class PowerUpController : MonoBehaviour {
	public PowerUpType type;
	public float duration;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init(PowerUpType type, float duration) {
        gameObject.transform.position = new Vector3(0.5f, 0.5f, -6.0f);

		// set the modifier

		// set velocity
		// add a collider (kinetic)?

		// after collision, call ball modifier
	}
}
