﻿using UnityEngine;
using System.Collections;

public class cameraMove : MonoBehaviour
{

	public int speed;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Translate (Vector3.forward * Time.deltaTime * -speed);
	
	}
}
