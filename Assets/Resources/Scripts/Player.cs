using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// player movement
	const float maxSpeed = 80f;
	const float deltaSpeed = 4f;
	float speed;
	bool isFacingLeft;

	// state machine
	enum State {Walking, Dead};
	State currentState;

	float prevFootstepTime;
	float footstepWaitTime;

	// user inputs
	string[] inputStrings = {"a", "d"};
  bool[] inputs;

  AudioSource[] audios;

	// Use this for initialization
	void Start () {
		inputs = new bool[inputStrings.Length];
		speed = maxSpeed;
		audios = GetComponents<AudioSource>();
		prevFootstepTime = Time.time;
		footstepWaitTime = 100000f;
	}


  // do physics stuff
  void FixedUpdate () {
  	if (inputs[0] || inputs[1]) {
  		Vector3 f = Vector3.forward;
	    if (inputs[0]) {
	    	f += Vector3.left;
	    } else {
	    	f += Vector3.right;
	    }
	    GetComponent<Rigidbody>().AddForce(f * speed);
  	}


		float x = GetComponent<Rigidbody>().velocity.x;
		if (!isFacingLeft && x < 0f) {
    	isFacingLeft = true;
    	Vector3 s = GetComponent<Transform>().localScale;
    	GetComponent<Transform>().localScale = new Vector3(-s.x, s.y, s.z);
    } else if (isFacingLeft && x > 0f) {
    	isFacingLeft = false;
    	Vector3 s = GetComponent<Transform>().localScale;
    	GetComponent<Transform>().localScale = new Vector3(-s.x, s.y, s.z);
    }

  	float r = GetComponent<Rigidbody>().velocity.magnitude / maxSpeed;
  	GetComponent<Animator>().speed = r * 8f;

  	if (r > 0.01f) {
  		footstepWaitTime = 0.04f / r;
	  	if (Time.time - prevFootstepTime > footstepWaitTime) {
	  		prevFootstepTime = Time.time;
	  		audios[UnityEngine.Random.Range(0, audios.Length)].Play();
	  	}
  	}
  	
  	

  	
  }

  void Die () {
  	GameObject.Find("Canvas/Death").GetComponent<DeathScreen>().Activate();
  }


  public void GetLatched () {
  	if (currentState != State.Dead) {
			speed = Mathf.Max(speed - deltaSpeed, 0f);
	  	if (speed <= 0f) {
	  		currentState = State.Dead;
	  		Invoke("Die", 5f);
	  		
	  	}
  	}
	  	
  }

  public void GetUnlatched () {
  	speed = Mathf.Min(speed + deltaSpeed, maxSpeed);
  }


	
	// Update is called once per frame
	void Update () {
		if (currentState == State.Dead) {
      return;
    }

    for (int i = 0; i < inputStrings.Length; i++) {
      inputs[i] = Input.GetKeyDown(inputStrings[i]);
    }
	}
}
