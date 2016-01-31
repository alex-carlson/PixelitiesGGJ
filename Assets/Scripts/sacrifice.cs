using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sacrifice : MonoBehaviour {

	public GameObject fire;
	public GameObject steam;
	public Material charredMat;

	public static int score;

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Pickup") {

			GameObject tik = GameObject.Find ("Anger");
			string theObject = GameObject.Find ("SacText").GetComponent<Text> ().text;
			Instantiate (fire, transform.position, Quaternion.Euler(Vector3.up));
			GameObject.Find ("Main Camera").GetComponent<grabObject> ().doShake ();

			if (col.transform.root.gameObject.name == theObject) {
				score++;
				GameObject.Find ("Score").GetComponent<Text> ().text = score + "";
				tik.GetComponent<tikiTimer> ().timeleft = tik.GetComponent<tikiTimer> ().timer;
				tik.GetComponent<tikiTimer> ().currStage++;
				tik.GetComponent<tikiTimer> ().update2 ();
				Destroy (col.transform.root.gameObject);
			} else {
				col.transform.position = transform.position;
				col.transform.GetComponentInChildren<Rigidbody> ().velocity = Vector3.zero;
				col.transform.GetComponentInChildren<Rigidbody> ().velocity = new Vector3(0, 1f, -1f) * 12;
				col.transform.GetComponentInChildren<MeshRenderer> ().material = charredMat;
			}
		}
	}
}
