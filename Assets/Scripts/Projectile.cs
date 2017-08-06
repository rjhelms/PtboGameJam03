using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public Vector3 MoveVector;
	public float gravRate;
	public bool hasGravity;
	public bool isFriendly;
	private float gravTime;
	private GameController controller;
	// Use this for initialization
	void Start () {
		controller = GameObject.FindWithTag("GameController")
				.GetComponent<GameController>();
		gravTime = Time.fixedTime + gravRate;
	}
	
	void FixedUpdate()
	{
		if (controller.IsRunning)
		{
			transform.localPosition += MoveVector;
			if (hasGravity && Time.fixedTime > gravTime)
			{
				MoveVector += Vector3.down;
				gravTime += gravRate;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "MainCamera")
		{
			Destroy(gameObject);
		} else if (isFriendly && other.tag == "Enemy")
		{
			Debug.Log("Hit enemy " + other);
			Destroy(gameObject);
			other.GetComponent<Enemy>().Hit();
		} else if (!isFriendly && other.tag == "Player" 
				   && !other.GetComponent<Player>().IsHitTimeout)
		{
			other.GetComponent<Player>().Hit();
			Destroy(gameObject);
		}
	}
}
