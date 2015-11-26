using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// player movement
	const float maxSpeed = 80f;
	const float deltaSpeed = 4f;
	float speed;

	// state machine
	enum State {Walking, Dead};
	State currentState;

	// user inputs
	string[] inputStrings = {"a", "d"};
  bool[] inputs;

	// Use this for initialization
	void Start () {
		inputs = new bool[inputStrings.Length];
		speed = maxSpeed;
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

  	GetComponent<Animator>().speed = GetComponent<Rigidbody>().velocity.magnitude / maxSpeed * 5f;
  }


  public void GetLatched () {
  	speed = Mathf.Max(speed - deltaSpeed, 0f);
  	if (speed <= 0f) {
  		currentState = State.Dead;
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
