using UnityEngine;
using System.Collections;

public class Cat : MonoBehaviour {

	float speed;
	GameObject player;
	enum State { RunToward, Latched };
	State currentState;



	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		speed = 5f;
	}



	void ChasePlayer (Vector3 d) {
		GetComponent<Rigidbody>().velocity = d.normalized * speed;
	}



	void StateTransitions () {
		Vector3 d = player.GetComponent<Transform>().position - GetComponent<Transform>().position;
		switch (currentState) {
			case State.RunToward:
				if (d.sqrMagnitude < 0.1f) { // RunToward => Latched
					currentState = State.Latched;
				} else {
					ChasePlayer(d);
				}
				break;
			case State.Latched:
				// TODO
				break;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		StateTransitions();
	}
}
