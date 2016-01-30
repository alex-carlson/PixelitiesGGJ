using UnityEngine;
using System.Collections;

public class destroySelf : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (destruct ());
	}

	IEnumerator destruct(){
		//yield return new WaitForSeconds (2);
		GetComponent<ParticleSystem> ().Stop ();
		yield return new WaitForSeconds (5);
		Destroy (this.transform.root.gameObject);
	}
}
