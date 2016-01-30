using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sacrifice : MonoBehaviour {

	public static int score;

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Pickup") {
			Destroy (col.gameObject);
			score++;
			GameObject.Find ("Score").GetComponent<Text> ().text = score+"";
		}
	}
}
