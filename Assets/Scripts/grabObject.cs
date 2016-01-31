using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
	public Sprite activeCursor;
	public Sprite inactiveCursor;
	GameObject cursor;
	LineRenderer lr;

	[Range(0, 1)] public float smoothSpeed = 0.1f;
	[Range(0, 1)] public float magnitude = 0.1f;
	[Range(0, 1)] public float shakeTime = 0.1f;

	private Vector3 velocity = Vector3.zero;

	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		cursor = GameObject.Find ("Cursor");
		lr = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		camFwd = Camera.main.transform;
		Vector3 fwd = transform.TransformDirection(Vector3.forward);

		RaycastHit hit;

//		lr.SetPosition (0, transform.position+Vector3.down);
//		lr.SetPosition(1, 
//			Vector3.Lerp(
//				transform.position, camFwd.position + camFwd.forward*7 + camFwd.up * 2, 0.5f
//			) 
//		);
//		lr.SetPosition(2, Camera.main.transform.position + camFwd.forward * 10);

		yaw += speedH * Input.GetAxis("Mouse X");
		pitch -= speedV * Input.GetAxis("Mouse Y");

		transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

		if (heldObject != null && isHolding == true) {
			
			heldObject.transform.position = Vector3.SmoothDamp (
				heldObject.transform.position,
				Camera.main.transform.position + (camFwd.forward * 4),
				ref velocity,
				0.1f
			);

			heldObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			heldObject.transform.rotation = Quaternion.identity;
		}

		if (Physics.Raycast(transform.position, fwd, out hit)) {
			if (hit.transform.tag == "Pickup") {

				cursor.GetComponent<Image> ().sprite = activeCursor;

				cursor.GetComponent<RectTransform> ().sizeDelta = Vector2.Lerp (
					cursor.GetComponent<RectTransform> ().sizeDelta,
					new Vector2(128, 128),
					smoothSpeed
				);

				cursor.GetComponent<RectTransform> ().localRotation = Quaternion.Slerp (
					cursor.GetComponent<RectTransform> ().localRotation,
					Quaternion.Euler(0, 0, 180),
					Time.deltaTime/smoothSpeed
				);

				// clicked on a pickupable thing
				if (Input.GetButtonDown ("Fire1")) {
					heldObject = hit.transform.gameObject;

					if (isHolding == false) {
						isHolding = true;
						if (heldObject.GetComponent<AudioSource> ()) {
							heldObject.GetComponent<AudioSource> ().Play ();
						}
					} else {
						heldObject.GetComponent<Rigidbody> ().AddForce ((camFwd.forward * force) * 120);
						isHolding = false;
					}
				}
			} else {
				cursor.GetComponent<Image> ().sprite = inactiveCursor;
				cursor.GetComponent<RectTransform> ().sizeDelta = Vector2.Lerp (
					cursor.GetComponent<RectTransform> ().sizeDelta,
					new Vector2(256, 256),
					smoothSpeed
				);
				cursor.GetComponent<RectTransform> ().localRotation = Quaternion.Slerp (
					cursor.GetComponent<RectTransform> ().localRotation,
					Quaternion.Euler(0, 0, 0),
					Time.deltaTime/smoothSpeed
				);
			}

		}
	}
	public void doShake(){
		//StartCoroutine (Shake ());
	}
	IEnumerator Shake() {

		float elapsed = 0.0f;

		Vector3 originalCamPos = Camera.main.transform.position;

		while (elapsed < shakeTime) {

			elapsed += Time.deltaTime;          

			float percentComplete = elapsed / shakeTime;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;

			Camera.main.transform.position = new Vector3(x, y, originalCamPos.z);

			yield return null;
		}

		Camera.main.transform.position = originalCamPos;
	}
}
