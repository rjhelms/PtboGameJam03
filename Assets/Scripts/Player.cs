using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	int moveSpeed = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 moveVector = new Vector2();
		if (Input.GetAxis("Horizontal") > 0)
		{
			moveVector += Vector2.right;
		} else if (Input.GetAxis("Horizontal") < 0)
		{
			moveVector += Vector2.left;
		}
		if (Input.GetAxis("Vertical") > 0)
		{
			moveVector += Vector2.up;
		} else if (Input.GetAxis("Vertical") < 0)
		{
			moveVector += Vector2.down;
		}
		moveVector *= moveSpeed;
		transform.position += (Vector3)moveVector;
	}
}
