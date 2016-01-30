using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class tikiTimer : MonoBehaviour {

	public float timer = 15f;
	public float timeleft;
	public int currStage = 4;
	public Sprite[] tikis;

	GameObject timeBar;

	// Use this for initialization
	void Start () {
		timeleft = timer;
		timeBar = GameObject.Find ("TimeLeft");
	}
	
	// Update is called once per frame
	void Update () {
		timeleft -= Time.deltaTime;
		timeBar.GetComponent<RectTransform> ().sizeDelta = new Vector2 (timeleft * 10, 20);

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

		if (currStage >= 0 && currStage < 5) {
			GetComponent<Image> ().sprite = tikis [currStage];
		}

		if (currStage == 0) {
			StartCoroutine (GameOver ());
		}

		yield return null;
	}
}
