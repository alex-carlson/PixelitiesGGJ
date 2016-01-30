using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class currentSacrifice : MonoBehaviour {

	public Sprite[] itemSprites;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateSacrifice(){
		Sprite thisSprite = itemSprites [(int)Mathf.Floor (Random.Range (0, itemSprites.Length))];
		GameObject.Find("SacText").GetComponent<Text> ().text = thisSprite.name;
		GameObject.Find("Item").GetComponent<SpriteRenderer>().sprite = thisSprite;
	}
}
