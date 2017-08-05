using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public Vector3 MoveVector;
	public float lifeSpan;
	private float dieTime;
	// Use this for initialization
	void Start () {
		dieTime = Time.fixedTime + lifeSpan;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		transform.position += MoveVector;
		if (Time.fixedTime > dieTime)
		{
			Destroy(gameObject);
		}
	}
}
