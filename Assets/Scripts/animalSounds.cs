using UnityEngine;
using System.Collections;

public class animalSounds : MonoBehaviour {

	public AudioClip pickupSound;
	public AudioClip throwSound;

	Rigidbody rb;
	AudioSource aud;
	bool hasPlayed = false;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		aud = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (rb.velocity.magnitude > 20f && hasPlayed == false) {
			StartCoroutine (throwAnimal ());
		}
	}

	public IEnumerator throwAnimal(){
		hasPlayed = true;
		aud.clip = throwSound;
		aud.Play ();
		StartCoroutine (revertSound ());
		yield return null;
	}

	IEnumerator revertSound(){
		yield return new WaitForSeconds (2);
		aud.clip = pickupSound;

		if (aud.isPlaying == false && rb.velocity.magnitude < 2f) {
			hasPlayed = false;
		}

		yield return null;
	}
}
