using UnityEngine;
using System.Collections;

public class grabObject : MonoBehaviour {

	void Start() {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		Debug.DrawLine (transform.position, fwd, Color.red);

		RaycastHit hit;

		if (Physics.Raycast(transform.position, transform.forward, out hit)) {
			if (hit.transform.tag == "Pickup") {
				Debug.Log (hit.transform.gameObject);
			}
		}
	}
}
