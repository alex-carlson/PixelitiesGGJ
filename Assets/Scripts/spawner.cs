using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {

	public GameObject[] throwables;
	//public Transform animalSpawn;

	void Start () {
		//animalSpawn = GameObject.Find ("AnimalSpawns").transform;
		InvokeRepeating ("spawnThings", 0f, 15f);
	}

	void spawnThings(){
		Vector3 randomVect = new Vector3 ( Random.Range(-3, 3), 0, Random.Range(-3, 3) );
		GameObject go = Instantiate(throwables [Mathf.FloorToInt (Random.Range (0, throwables.Length))], transform.position+randomVect, Quaternion.identity) as GameObject;
		//go.transform.parent = animalSpawn;
	}
}
