using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {

	public GameObject[] throwables;

	void Start () {
		InvokeRepeating ("spawnThings", 10f, 5f);
	}

	void spawnThings(){
		Vector3 randomVect = new Vector3 ( Random.Range(-15, 15), 0, Random.Range(-15, 15) );
		Instantiate (throwables [Mathf.FloorToInt (Random.Range (0, throwables.Length))], transform.position+randomVect, Quaternion.identity);
	}
}
