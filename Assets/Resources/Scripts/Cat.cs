using UnityEngine;
using System.Collections;

public class Cat : MonoBehaviour {

	float speed;
	bool isFacingLeft;
	
	GameObject player;

	enum State { RunToward, Latched };
	State currentState;

	Vector3 prevPlayerPosition;
	float prevMeowTime;
	float meowWaitTime;

	AudioSource[] audios;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		speed = 5f;
		isFacingLeft = true;
		audios = GetComponents<AudioSource>();
		meowWaitTime = NewMeowWaitTime;
	}



	void ChasePlayer (Vector2 d) {
		Vector2 v = d.normalized * speed;
		GetComponent<Rigidbody>().velocity = new Vector3(v.x, 0f, v.y);
		float x = d.x;

		if (!isFacingLeft && x < 0f) {
    	isFacingLeft = true;
    	Vector3 s = GetComponent<Transform>().localScale;
    	GetComponent<Transform>().localScale = new Vector3(-s.x, s.y, s.z);
    } else if (isFacingLeft && x > 0f) {
    	isFacingLeft = false;
    	Vector3 s = GetComponent<Transform>().localScale;
    	GetComponent<Transform>().localScale = new Vector3(-s.x, s.y, s.z);
    }
	}


	float RandomEuler { get { return UnityEngine.Random.Range(20f, 70f); } }
	float RandomHeight { get { return UnityEngine.Random.Range(0f, 3f); } }
	float NewMeowWaitTime { get { return UnityEngine.Random.Range(5f, 20f); } }

	void LatchOn () {
		currentState = State.Latched;
		GetComponent<Animator>().SetTrigger("Latch");
		GetComponent<Collider>().enabled = false;
		GetComponent<Rigidbody>().useGravity = false;

		// get rotation right
		Vector3 e = new Vector3(0f, 0f, RandomEuler);
		if (GetComponent<Rigidbody>().velocity.x < 0f) {
			e = new Vector3(0f, 0f, 90f + e.z);
		}
		GetComponent<Transform>().eulerAngles = e;

		prevPlayerPosition = player.GetComponent<Transform>().position;
		Vector3 p = GetComponent<Transform>().position;
		GetComponent<Transform>().position = new Vector3(p.x, p.y + RandomHeight , p.z);
		GetComponent<Rigidbody>().velocity = Vector3.zero;

    // put cat in the back
		GetComponent<SpriteRenderer>().sortingOrder = 1;
		player.GetComponent<Player>().GetLatched();

		audios[audios.Length - 1].Play();
	}


	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.tag == "Player") {
			LatchOn();
		}
	}


	void StateTransitions () {
		Vector3 pp = player.GetComponent<Transform>().position;
		Vector3 d3 = pp - GetComponent<Transform>().position;
		Vector2 d = new Vector2(d3.x, d3.z);
		switch (currentState) {
			case State.RunToward:
				if (d.sqrMagnitude < 0.3f) {
					LatchOn();
				} else {
					ChasePlayer(d);
				}
				break;
			case State.Latched:
				GetComponent<Transform>().position += pp - prevPlayerPosition;
				prevPlayerPosition = pp;
				break;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		StateTransitions();
		if(Time.time - prevMeowTime > meowWaitTime) {
			audios[UnityEngine.Random.Range(0, audios.Length - 1)].Play();
			prevMeowTime = Time.time;
			meowWaitTime = NewMeowWaitTime;
		}
	}
}
