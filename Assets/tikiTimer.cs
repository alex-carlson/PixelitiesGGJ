using UnityEngine;
using System.Collections;

public class tikiTimer : MonoBehaviour {

	public float timer = 15f;
	float timeleft;

	// Use this for initialization
	void Start () {
		timeleft = timer;
	}
	
	// Update is called once per frame
	void Update () {
		timeleft -= Time.deltaTime;

		Debug.Log (timeleft);

		if (timeleft <= 0) {
			timeleft = timer;
			GetComponent<Animation> ().Play ();
		}
	}
}
