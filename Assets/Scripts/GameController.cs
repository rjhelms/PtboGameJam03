using System.Collections;
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

	[Header("Game Balance")]
	public int ScrollRate;
	public int ScrollFrames;
	public int ScrollScore = 10;

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

	private float pixelRatioAdjustment;
	private int scrollFrame = 0;
	private int nextGenXPosition = 16;
	private int terrainHeight = 1;

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
	}
	
	// Update is called once per frame
	void Update () {
		if (WorldCamera.transform.position.x >= nextGenXPosition)
		{
			GenerateTile();
			nextGenXPosition += GridSizeX;
		}
		ScoreText.text = string.Format("SCORE\n{0}",
									   ScoreManager.Instance.Score);
	}

	void FixedUpdate()
	{
		scrollFrame++;
		scrollFrame = scrollFrame % ScrollFrames;
		if (scrollFrame == 0)
		{
			Vector3 scrollVector = Vector2.right * ScrollRate;
			WorldCamera.transform.position += scrollVector;
			ScoreManager.Instance.Score += ScrollRate * ScrollScore;
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
		Destroy(enemy);
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
			Debug.Log(enemyIndex);

			// spawn enemy
			YPos = GenYOffset + terrainHeight * GridSizeY;
			Instantiate(Enemies[enemyIndex], new Vector3(XPos, YPos, 0),
						Quaternion.identity, EnemyContainer);
		}
	}
}
