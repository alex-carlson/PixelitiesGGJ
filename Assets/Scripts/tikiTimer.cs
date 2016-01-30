using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class tikiTimer : MonoBehaviour {

	public float timer = 15f;
	public float timeleft;
	public int currStage = 5;
	public Sprite[] tikis;

	// Use this for initialization
	void Start () {
		timeleft = timer;
	}
	
	// Update is called once per frame
	void Update () {
		timeleft -= Time.deltaTime;

		if (timeleft <= 0) {
			timeleft = timer;
			GetComponent<Animation> ().Play ();
			GameObject.Find ("SacText").GetComponent<currentSacrifice> ().UpdateSacrifice ();
			currStage--;

			if (currStage >= 0) {
				GetComponent<Image> ().sprite = tikis [currStage];
			}

			if (currStage == 0) {
				StartCoroutine (GameOver ());
			}
		}
	}

	public void update2(){
		StartCoroutine (doStuff ());
	}

	IEnumerator GameOver(){
		Time.timeScale = 0.5f;
		GameObject.Find ("You").GetComponent<Animation> ().Play ();
		GameObject.Find ("Lose").GetComponent<Animation> ().Play ();
		yield return null;
	}

	IEnumerator doStuff(){
		timeleft = timer;
		GetComponent<Animation> ().Play ();
		GameObject.Find ("SacText").GetComponent<currentSacrifice> ().UpdateSacrifice ();

		if (currStage >= 0) {
			GetComponent<Image> ().sprite = tikis [currStage];
		}

		if (currStage == 0) {
			StartCoroutine (GameOver ());
		}

		yield return null;
	}
}
