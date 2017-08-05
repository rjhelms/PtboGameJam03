using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private GameController controller;

	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag
			("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Hit()
	{
		Debug.Log("Ouch!");
		controller.SpawnHippie(gameObject);
	}
}
