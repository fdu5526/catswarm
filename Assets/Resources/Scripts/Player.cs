using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	float speed;

	enum State {Walking, Dead};
	State currentState;

	string[] inputStrings = {"a", "d"};
  bool[] inputs;

	// Use this for initialization
	void Start () {
		inputs = new bool[inputStrings.Length];
		speed = 10f;
	}


  // do physics stuff
  void FixedUpdate () {
    if (inputs[0] || inputs[1]) {
    	GetComponent<Rigidbody>().AddForce(Vector3.forward * speed);
    }
  }
	
	// Update is called once per frame
	void Update () {
		if (currentState == State.Dead) {
      return;
    }
    for (int i = 0; i < inputStrings.Length; i++) {
      inputs[i] = Input.GetKey(inputStrings[i]);
    }
	}
}
