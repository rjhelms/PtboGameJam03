﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public int moveSpeedX = 1;
	public int moveSpeedY = 2;
	public Transform FirePoint;
	public Transform ProjectileParent;
	public GameObject[] StraightProjectiles;
	public GameObject[] GravProjectiles;
	public float FireInterval = 0.2f;
	private float[] nextFireTime = new float[2];
	private int fireIndex = 0;

	// Use this for initialization
	void Start () {
		nextFireTime[0] = Time.fixedTime;
		nextFireTime[1] = Time.fixedTime;
	}

	void FixedUpdate () {
		Vector2 moveVector = new Vector2();
		if (Input.GetAxis("Horizontal") > 0
			& transform.localPosition.x < 36)
		{
			moveVector += Vector2.right * moveSpeedX;
		} else if (Input.GetAxis("Horizontal") < 0
				   & transform.localPosition.x > -80)
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
		transform.localPosition += (Vector3)moveVector;
		if (Input.GetButton("Fire1") & Time.fixedTime > nextFireTime[0])
		{
			GameObject.Instantiate(StraightProjectiles[fireIndex],
								   FirePoint.position, Quaternion.identity,
								   ProjectileParent);
			nextFireTime[0] = Time.fixedTime + FireInterval;
			fireIndex++;
			fireIndex = fireIndex % StraightProjectiles.Length;
		}
		if (Input.GetButton("Fire2") & Time.fixedTime > nextFireTime[1])
		{
			GameObject.Instantiate(GravProjectiles[fireIndex],
								   FirePoint.position, Quaternion.identity,
								   ProjectileParent);
			nextFireTime[1] = Time.fixedTime + FireInterval;
			fireIndex++;
			fireIndex = fireIndex % StraightProjectiles.Length;
		}
	}
}
