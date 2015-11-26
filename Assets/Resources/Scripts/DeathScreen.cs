using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathScreen : MonoBehaviour {

	float t;
	bool activated;

	// Use this for initialization
	void Start () {
		GetComponent<Image>().color = Color.clear;
		t = 0f;
	 	activated = false;
	}


	public void Activate () {
		activated = true;
		GetComponent<AudioSource>().Play();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (activated && t < 1f) {
			t += 0.005f;
			GetComponent<Image>().color = new Color(1f, 1f, 1f, t);
		}
	}
}
