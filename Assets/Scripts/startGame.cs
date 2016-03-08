using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Rewired;

public class startGame : MonoBehaviour {

	private Player player;
	public int playerid = 0;


	// Use this for initialization
	void Start () {
		player = ReInput.players.GetPlayer(playerid);
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetButtonDown("Reset")) {
			SceneManager.LoadScene (1);
		}
	}
}
