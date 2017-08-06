using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public Vector3 MoveVector;
	public float lifeSpan;
	public float gravRate;
	public bool hasGravity;
	public bool isFriendly;
	private float dieTime;
	private float gravTime;
	// Use this for initialization
	void Start () {
		dieTime = Time.fixedTime + lifeSpan;
		gravTime = Time.fixedTime + gravRate;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		transform.localPosition += MoveVector;
		// if (Time.fixedTime > dieTime)
		// {
		// 	Destroy(gameObject);
		// }
		if (hasGravity & Time.fixedTime > gravTime)
		{
			MoveVector += Vector3.down;
			gravTime += gravRate;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "MainCamera")
		{
			Destroy(gameObject);
		} else if (isFriendly & other.tag == "Enemy")
		{
			Debug.Log("Hit enemy " + other);
			Destroy(gameObject);
			other.GetComponent<Enemy>().Hit();
		} else if (!isFriendly & other.tag == "Player")
		{
			Debug.Log("Yoo loooooz");
			Debug.Break();
		}
	}
}
