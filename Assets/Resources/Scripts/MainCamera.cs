using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	GameObject player;
	Vector3 prevPlayerPosition;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").gameObject;
		prevPlayerPosition = player.GetComponent<Transform>().position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 p = player.GetComponent<Transform>().position;


		GetComponent<Transform>().position += (p - prevPlayerPosition);
		prevPlayerPosition = p;

	}
}
