﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	[Header("Camera Properties")]
	public Camera WorldCamera;
	public Material WorldCameraTexture;
	public int TargetX;
	public int TargetY;
	public Text ScoreText;
	public Image HealthImage;
	public int HealthImageUnitX = 12;
	public int HealthImageUnitY = 16;
	public GameObject TitlePanel;

	[Header("Game Balance")]
	public int ScrollRate;
	public int ScrollFrames;
	public int ScrollScoreDistance = 50;
	public int ScrollScorePoints = 10;
	public int PointsSpeed = 5000;
	public float PointsSpeedScale = 1.1f;
	public int PointsDensity = 2500;
	public float PointsDensityScale = 1.05f;
	public float PointsDensityFactor = 1.01f;
	public int PointsHitPoints = 5000;
	public float PointsHitPointsScale = 1.1f;
	public bool IsRunning;
	public bool IsStarting = false;
	public bool IsLosing = false;
	public float StartDelay = 0.1f;

	[Header("Global Prefabs")]
	public GameObject[] Hippies;
	public GameObject[] Enemies;
	public GameObject BaseTerrain;

	[Header("Containters")]
	public Transform HippieContainer;
	public Transform EnemyContainer;
	public Transform TerrainContainer;

	[Header("World Gen Properties")]
	public int GridSizeX = 16;
	public int GridSizeY = 16;
	public int GenXOffset = 96;
	public int GenYOffset = -100;
	public float TerrainHeightChangeChance = 0.25f;
	public float EnemyDensity = 0.05f;
	public int TerrainHeightMin = 1;
	public int TerrainHeightMax = 6;
	
	[Header("Audio")]
	public AudioSource FXPlayer;
	public AudioClip FXPlayerFire;
	public AudioClip FXSoldierFire;
	public AudioClip FXSuitFire;
	public AudioClip FXPlayerHit;
	public AudioClip FXEnemyHit;
	public AudioClip FXSpeedUp;
	public AudioClip FXHPUp;
	public AudioClip FXGameOver;
	public AudioClip FXBlip;
	public GameObject MusicPlayer;

	private float pixelRatioAdjustment;
	private int scrollFrame = 0;
	private int nextGenXPosition = 16;
	private int terrainHeight = 1;
	private int scrollSinceLastScore;
	private float startTime;

	// Use this for initialization
	void Start () {
		pixelRatioAdjustment = (float)TargetX / (float)TargetY;
        if (pixelRatioAdjustment <= 1)
        {
            WorldCameraTexture.mainTextureScale = 
				new Vector2(pixelRatioAdjustment, 1);
            WorldCameraTexture.mainTextureOffset =
				new Vector2((1 - pixelRatioAdjustment) / 2, 0);
            WorldCamera.orthographicSize = TargetY / 2;
        }
        else
        {
            pixelRatioAdjustment = 1f / pixelRatioAdjustment;
            WorldCameraTexture.mainTextureScale = 
				new Vector2(1, pixelRatioAdjustment);
            WorldCameraTexture.mainTextureOffset = 
				new Vector2(0, (1 - pixelRatioAdjustment) / 2);
            WorldCamera.orthographicSize = TargetX / 2;
        }
		if (GameObject.FindWithTag("MusicPlayer") == null)
		{
			GameObject musicPlayer = Instantiate(MusicPlayer, Vector3.zero,
											Quaternion.identity);
			Object.DontDestroyOnLoad(musicPlayer);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!IsLosing)
			{
			if (!IsStarting && !IsRunning)
			{
				if (Input.anyKeyDown)
                {
					IsStarting = true;
					TitlePanel.SetActive(false);
					startTime = Time.time + StartDelay;
				}
			} else if (IsStarting && !IsRunning)
			{
				if (Time.time > startTime)
				{
					IsRunning = true;
					IsStarting = false;
				}
			} else if (!IsStarting && IsRunning)
			{
				while (WorldCamera.transform.position.x >= nextGenXPosition)
				{
					GenerateTile();
					nextGenXPosition += GridSizeX;
				}
				ScoreText.text = string.Format("SCORE\n{0}",
											ScoreManager.Instance.Score);
				HealthImage.rectTransform.sizeDelta = 
					new Vector2(ScoreManager.Instance.HitPoints * HealthImageUnitX,
								HealthImageUnitY);
			}
		}
	}

	void FixedUpdate()
	{
		if (IsRunning)
		{
			scrollFrame++;
			scrollFrame = scrollFrame % ScrollFrames;
			if (scrollFrame == 0)
			{
				Vector3 scrollVector = Vector2.right * ScrollRate;
				WorldCamera.transform.position += scrollVector;
				scrollSinceLastScore += ScrollRate;
				if (scrollSinceLastScore >= ScrollScoreDistance)
				{
					ScoreManager.Instance.Score += ScrollScorePoints;
					scrollSinceLastScore = 0;
				}
			}
			if (ScoreManager.Instance.Score >= PointsSpeed)
			{
				IncreaseSpeed();
			}
			if (ScoreManager.Instance.Score >= PointsDensity)
			{
				IncreaseDensity();
			}
			if (ScoreManager.Instance.Score >= PointsHitPoints)
			{
				IncreaseHitPoints();
			}
		}
	}

	public void SpawnHippie(GameObject enemy, int points)
	{
		int hippieIndex = Random.Range(0, Hippies.Length);
		GameObject newHippie = Instantiate(Hippies[hippieIndex],
										   enemy.transform.position,
										   Quaternion.identity,
										   HippieContainer);
		if (Random.value < 0.5f) // 50% chance will spawn flipped L/R
		{
			newHippie.transform.localScale = new Vector3(-1, 1, 1);
			newHippie.transform.position += new Vector3(16, 0, 0);
		}
		ScoreManager.Instance.Score += points;
		FXPlayer.PlayOneShot(FXEnemyHit);
		Destroy(enemy);
	}

	public void TakeDamage()
	{
		if (ScoreManager.Instance.HitPoints > 0)
		{
			ScoreManager.Instance.HitPoints--;
			FXPlayer.PlayOneShot(FXPlayerHit);
		} else {
			FXPlayer.PlayOneShot(FXGameOver);
			Debug.Log("Game over!");
			Lose();
		}
	}

	void GenerateTile()
	{
		// check for height change
		float changeHeight = Random.value;
		if (changeHeight < TerrainHeightChangeChance)
		{
			float increase = Random.value;
			if (terrainHeight == TerrainHeightMax | 
				(increase < 0.5f & terrainHeight > TerrainHeightMin))
			{
				terrainHeight--;
				if (terrainHeight < TerrainHeightMin)
				{
					terrainHeight++;
				}
			} else {
				terrainHeight++;
				if (terrainHeight > TerrainHeightMax)
				{
					terrainHeight--;
				}
			}
		}
		// generate terrain
		float XPos = nextGenXPosition + GenXOffset;
		float YPos = GenYOffset;
		for (int i = 0; i < terrainHeight; i++)
		{
			Instantiate(BaseTerrain, new Vector3(XPos, YPos, 0), 
						Quaternion.identity, TerrainContainer);
			YPos += GridSizeY;
		}
		// test for enemy generation
		float genEnemy = Random.value;
		if (genEnemy < EnemyDensity)
		{
			// if yes, determine enemy
			float enemyRand = Random.Range(1, Mathf.Exp(Enemies.Length));
			int enemyIndex = Mathf.FloorToInt(Mathf.Log(enemyRand));
			enemyIndex = Enemies.Length - enemyIndex;
			enemyIndex--;

			// spawn enemy
			YPos = GenYOffset + terrainHeight * GridSizeY;
			Instantiate(Enemies[enemyIndex], new Vector3(XPos, YPos, 0),
						Quaternion.identity, EnemyContainer);
		}
	}

	void IncreaseSpeed()
	{
		if (ScrollFrames > 1)
		{
			ScrollFrames--;
		} else {
			ScrollRate++;
		}
		PointsSpeed = Mathf.RoundToInt(Mathf.Pow(PointsSpeed,
												 PointsSpeedScale));
		Debug.Log("Next speed increase: " + PointsSpeed);
		FXPlayer.PlayOneShot(FXSpeedUp);
	}

	void IncreaseDensity()
	{
		float lnDensity = Mathf.Log(EnemyDensity);
		lnDensity /= PointsDensityFactor;
		EnemyDensity = Mathf.Exp(lnDensity);
		PointsDensity = Mathf.RoundToInt(Mathf.Pow(PointsDensity,
												   PointsDensityScale));
		Debug.Log("Next density increase: " + PointsDensity);
	}

	void IncreaseHitPoints()
	{
		if (ScoreManager.Instance.HitPoints < ScoreManager.Instance.MaxHitPoints)
		{
			ScoreManager.Instance.HitPoints++;
		}
		PointsHitPoints = Mathf.RoundToInt(Mathf.Pow(PointsHitPoints,
													 PointsHitPointsScale));
		Debug.Log("Next HP increase: " + PointsHitPoints);
		FXPlayer.PlayOneShot(FXHPUp);
	}

	void Lose()
	{
		IsRunning = false;
		IsLosing = true;
		GetComponent<GameOverController>().enabled = true;
	}
}
