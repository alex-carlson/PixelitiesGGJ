﻿using UnityEngine;
using System.Collections;

public class animalSounds : MonoBehaviour {

	public AudioClip pickupSound;
	public AudioClip throwSound;

	Rigidbody rb;
	AudioSource aud;
	bool hasPlayed = false;
	public bool isHeld = false;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		aud = GetComponent<AudioSource> ();
		StartCoroutine (destroyAfterSecs ());
	}
	
	// Update is called once per frame
	void Update () {
		if (rb.velocity.magnitude > 20f && hasPlayed == false) {
			StartCoroutine (throwAnimal ());
		}

		if (isHeld == true) {
			StopCoroutine (destroyAfterSecs ());
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

	IEnumerator destroyAfterSecs(){
		yield return new WaitForSeconds (45);
		if (isHeld == false) {
			Destroy (this.transform.root.gameObject);
		}
		yield return null;
	}
}
