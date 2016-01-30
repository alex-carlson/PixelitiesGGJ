using UnityEngine;
using System.Collections;

public class animalSounds : MonoBehaviour {

	public AudioClip pickupSound;
	public AudioClip throwSound;

	Rigidbody rb;
	AudioSource aud;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		aud = GetComponent<AudioSource> ();
		StartCoroutine (idleSound ());
	}
	
	// Update is called once per frame
	void Update () {
		if (rb.velocity.magnitude > 2f) {
			StartCoroutine (throwAnimal());
		}
	}

	public IEnumerator throwAnimal(){
		Debug.Log ("throwing");
		aud.clip = throwSound;
		aud.Play ();
		yield return null;
	}

	public IEnumerator idleSound(){
		yield return new WaitForSeconds(Mathf.Round(Random.Range(15, 30)));
		aud.Play ();
		StartCoroutine (idleSound ());
	}
}
