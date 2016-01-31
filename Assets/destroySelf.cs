using UnityEngine;
using System.Collections;

public class destroySelf : MonoBehaviour {

	public Component[] particles;

	// Use this for initialization
	void Start () {
		StartCoroutine (destruct ());
	}

	IEnumerator destruct(){
		//yield return new WaitForSeconds (2);

		particles = GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particle in particles) {
			particle.Stop ();
		}
//		yield return new WaitForSeconds (5);
//		Destroy (this.transform.root.gameObject);
		yield return null;
	}

	public void explode(){
		StartCoroutine (startPart ());
	}

	IEnumerator startPart(){

		particles = GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particle in particles) {
			particle.Play ();
		}

		yield return new WaitForSeconds (2);

		particles = GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particle in particles) {
			particle.Stop ();
		}
	}
}
