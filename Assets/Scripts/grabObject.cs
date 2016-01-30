using UnityEngine;
using System.Collections;

public class grabObject : MonoBehaviour {

	public float speedH = 2.0f;
	public float speedV = 2.0f;

	private float yaw = 0.0f;
	private float pitch = 0.0f;

	public float force = 10;

	GameObject heldObject = null;
	public float arcHeight = 10;

	bool isHolding = false;
	Transform camFwd;

	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
		camFwd = Camera.main.transform;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);

		RaycastHit hit;

		if (Physics.Raycast(transform.position, fwd, out hit)) {
			if (hit.transform.tag == "Pickup") {

				// clicked on a pickupable thing
				if (Input.GetButtonDown ("Fire1")) {
					heldObject = hit.transform.gameObject;

					if (isHolding == false) {
						isHolding = true;
					} else {
						heldObject.GetComponent<Rigidbody> ().AddForce (camFwd.forward * force * 20);
						isHolding = false;
					}
				}
			} else {
				// didn't click on a pickupable thing
				if(Input.GetButtonDown("Fire1")){
					
				}
			}

		}

		yaw += speedH * Input.GetAxis("Mouse X");
		pitch -= speedV * Input.GetAxis("Mouse Y");

		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

		if (heldObject != null && isHolding == true) {
			heldObject.transform.position = Camera.main.transform.position + (camFwd.forward * 4);
		}
	}
}
