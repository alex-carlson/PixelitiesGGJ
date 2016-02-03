using UnityEngine;
using System.Collections;

public class virginJetpack : MonoBehaviour {


	[ Range(5, 500) ] public float maxSpeed = 150;
	[ Range(5, 600) ] public float acceleration = 5;

	Rigidbody rb;
	Quaternion dir;
	float dist;
	Vector3 playerPos;
	Animator ani;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		InvokeRepeating ("TurnRandom", 0, 3f);
		dir = Quaternion.Euler (0, 0, 0);
		ani = GetComponent<Animator> ();
		ani.speed = 0;
		playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		dist = Vector3.Distance (transform.position, playerPos);

		if (dist < 10) {

			ani.speed = 1;
			
			if (rb.velocity.magnitude < maxSpeed) {
				rb.AddForce (transform.forward * acceleration);
				rb.AddForce (transform.forward * acceleration);
			}
		} else {
			ani.speed = 0.2f;
		}
		transform.rotation = dir;
	}

	void TurnRandom(){
		dir = Quaternion.Euler (0, Random.Range (0, 359), 0);
	}
}
