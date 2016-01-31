using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {

	public GameObject[] throwables;

	void Start () {
		InvokeRepeating ("spawnThings", 10f, 5f);
	}

	void spawnThings(){
		Instantiate (throwables [Mathf.FloorToInt (Random.Range (0, throwables.Length))], transform.position, Quaternion.identity);
	}
}
