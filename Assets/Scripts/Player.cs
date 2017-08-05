using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public int moveSpeedX = 1;
	public int moveSpeedY = 2;
	public Transform FirePoint;
	public GameObject[] StraightProjectiles;
	public float FireInterval = 0.2f;
	private float nextFireTime;
	// Use this for initialization
	void Start () {
		nextFireTime = Time.fixedTime;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 moveVector = new Vector2();
		if (Input.GetAxis("Horizontal") > 0
			& transform.position.x < 36)
		{
			moveVector += Vector2.right * moveSpeedX;
		} else if (Input.GetAxis("Horizontal") < 0
				   & transform.position.x > -80)
		{
			moveVector += Vector2.left * moveSpeedX;
		}
		if (Input.GetAxis("Vertical") > 0
			& transform.localPosition.y < 68)
		{
			moveVector += Vector2.up * moveSpeedY;
		} else if (Input.GetAxis("Vertical") < 0 
				   & transform.localPosition.y > -100)
		{
			moveVector += Vector2.down * moveSpeedY;
		}
		transform.position += (Vector3)moveVector;
		if (Input.GetButton("Fire1") & Time.fixedTime > nextFireTime)
		{
			int projectileIndex = Random.Range(0, StraightProjectiles.Length);
			GameObject.Instantiate(StraightProjectiles[projectileIndex],
								   FirePoint.position, Quaternion.identity);
			nextFireTime = Time.fixedTime + FireInterval;
		}
	}
}
