using UnityEngine;
using System.Collections;

public class virginJetpack : MonoBehaviour {


	[ Range(5, 500) ] public float maxSpeed = 150;
	[ Range(5, 600) ] public float acceleration = 5;

	Rigidbody rb;
	Quaternion dir;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		InvokeRepeating ("TurnRandom", 0, 3f);
		dir = Quaternion.Euler (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (rb.velocity.magnitude < maxSpeed) {
			rb.AddForce (transform.forward * acceleration);
			rb.AddForce (transform.forward * acceleration);
		}
		transform.rotation = dir;
	}

	void TurnRandom(){
		Debug.Log ("ay");
		dir = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, Random.Range (0, 359), 0), 0.3f);
	}
}
