using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {

	public GameObject[] throwables;

	void Start () {
		InvokeRepeating ("spawnThings", 0f, 15f);
	}

	void spawnThings(){
		Vector3 randomVect = new Vector3 ( Random.Range(-3, 3), 0, Random.Range(-3, 3) );
		GameObject go = Instantiate(throwables [Mathf.FloorToInt (Random.Range (0, throwables.Length))], transform.position+randomVect, Quaternion.identity) as GameObject;
		go.transform.parent = GameObject.Find ("AnimalSpawns").transform;
	}
}
