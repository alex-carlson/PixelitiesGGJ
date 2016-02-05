using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Rewired;
using UnityEngine.SceneManagement;

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

	private Player player;
	public int playerid = 0;

	void Awake(){
		player = ReInput.players.GetPlayer(playerid);
	}

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

		pitch += player.GetAxis ("Look Horizontal") * speedH;
		yaw -= player.GetAxis ("Look Vertical") * speedV;

		transform.rotation = Quaternion.Euler (yaw, pitch, 0);

		if (heldObject != null && isHolding == true) {

			heldObject.transform.position = Vector3.SmoothDamp (
				heldObject.transform.position,
				camFwd.position + (camFwd.forward * 4),
				ref velocity,
				0.1f
			);

			heldObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			//heldObject.transform.rotation = Quaternion.identity;
		}

		if (Physics.Raycast(transform.position, fwd, out hit)) {
			if (hit.transform.tag == "Pickup" && hit.distance < 8) {

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
				if (player.GetButtonDown ("Use")) {
					heldObject = hit.transform.gameObject;

					if (isHolding == false) {
						isHolding = true;
						heldObject.GetComponent<animalSounds> ().isHeld = true;
						if (heldObject.GetComponent<AudioSource> ()) {
							heldObject.GetComponent<AudioSource> ().Play ();
						}
						if (GetComponent<Animator> ()) {
							GetComponent<Animator> ().enabled = false;
						}
					} else {
						heldObject.GetComponent<Rigidbody> ().AddForce ((transform.forward * force) * 180);
						isHolding = false;
						StartCoroutine (Shake ());
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

		if (player.GetButtonDown ("Reset")) {
			SceneManager.LoadScene (0);
		}
	}

	public void doShake(){
		StartCoroutine (Shake ());
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
			float z = Random.value * 2.0f - 1.0f;
			x *= magnitude;
			z *= magnitude;

			Camera.main.transform.localPosition = new Vector3(x, originalCamPos.y, z);

			yield return null;
		}

		Camera.main.transform.position = originalCamPos;
	}
}