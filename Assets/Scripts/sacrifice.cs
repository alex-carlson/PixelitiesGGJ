using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sacrifice : MonoBehaviour {

	public static int score;

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Pickup") {

			GameObject tik = GameObject.Find ("Anger");
			string theObject = GameObject.Find ("SacText").GetComponent<Text> ().text;

			if (col.transform.root.gameObject.name == theObject) {
				score++;
				GameObject.Find ("Score").GetComponent<Text> ().text = score + "";
				tik.GetComponent<tikiTimer> ().timeleft = tik.GetComponent<tikiTimer> ().timer;
				tik.GetComponent<tikiTimer> ().currStage++;
				tik.GetComponent<tikiTimer> ().update2 ();
			}

			Destroy (col.transform.root.gameObject);
		}
	}
}
