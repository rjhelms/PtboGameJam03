using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private GameController controller;

	// Use this for initialization
	protected virtual void Start () {
		controller = GameObject.FindGameObjectWithTag
			("GameController").GetComponent<GameController>();
	}

	public void Hit()
	{
		controller.SpawnHippie(gameObject);
	}
}
