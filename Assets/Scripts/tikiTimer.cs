using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class tikiTimer : MonoBehaviour {

	public float timer = 15f;
	public float timeleft;
	public int currStage = 5;
	public Sprite[] tikis;
	public GameObject[] particles;
	public Camera playerCam;

	public Color[] skyboxColors;
	AudioSource gameMusic;
	public AudioClip[] musics;

	GameObject timeBar;

	// Use this for initialization
	void Start () {
		timeleft = timer;
		timeBar = GameObject.Find ("TimeLeft");
		StartCoroutine (doStuff ());
		gameMusic = GameObject.Find ("Game Music").GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		timeleft -= Time.deltaTime;
		timeBar.GetComponent<RectTransform> ().sizeDelta = new Vector2 (timeleft * 10, 20);

		if (timeleft <= 0) {
			timeleft = timer;
			GetComponent<Animation> ().Play ();

			if (currStage > -1) {
				GameObject.Find ("SacText").GetComponent<currentSacrifice> ().UpdateSacrifice ();
				GameObject.Find ("VolcanoTrigger").GetComponent<sacrifice> ().fire = particles [currStage];
			}

			if (currStage > 4) {

			} else {
				currStage--;
			}
				
			if (currStage == -1) {
				gameMusic.clip = musics [3];
				gameMusic.Play ();
				gameMusic.loop = false;
				StartCoroutine (GameOver ());
				return;
			}

			if (currStage >= 0 && currStage < 5) {
				StartCoroutine (fadeTo ());
				GetComponent<Image> ().sprite = tikis [currStage];
			}

			if (currStage == 4) {
				gameMusic.clip = musics [0];
				gameMusic.Play ();
			} else if (currStage == 2) {
				gameMusic.clip = musics [1];
				gameMusic.Play ();
			} else if (currStage == 0) {
				gameMusic.clip = musics [2];
				gameMusic.Play ();
			}
		}
	}

	public void update2(){
		StartCoroutine (doStuff ());
	}

	IEnumerator GameOver(){

		GameObject.Find ("You").GetComponent<Animation> ().Play ();
		GameObject.Find ("Lose").GetComponent<Animation> ().Play ();

		float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
		float increment = 0.1f/2f; //The amount of change to apply.
		Color fadeCol = GameObject.Find ("fade").GetComponent<Image> ().color;

		while(progress < 1)
		{

			fadeCol = Color.Lerp (fadeCol, new Color(0, 0, 0, 0.5f), progress);
			progress += increment;
			yield return new WaitForSeconds(0.1f);
		}

		return true;
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

	IEnumerator fadeTo(){

		float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
		float increment = 0.1f/2f; //The amount of change to apply.
		while(progress < 1)
		{
			playerCam.backgroundColor = Color.Lerp (playerCam.backgroundColor, skyboxColors[currStage], progress);
			progress += increment;
			yield return new WaitForSeconds(0.1f);
		}
		return true;
	}
}
