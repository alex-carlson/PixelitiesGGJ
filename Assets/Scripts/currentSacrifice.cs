using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class currentSacrifice : MonoBehaviour {

	public string[] items;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateSacrifice(){
		GameObject.Find("SacText").GetComponent<Text> ().text = items[(int)Mathf.Floor(Random.Range (0, items.Length))];
	}
}
