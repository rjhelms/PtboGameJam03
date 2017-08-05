using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	[Header("Camera Properties")]
	public Camera WorldCamera;
	public Material WorldCameraTexture;
	public int TargetX;
	public int TargetY;

	[Header("Game Balance")]
	public int ScrollRate;
	public int ScrollFrames;

	[Header("Global Prefabs")]
	public GameObject[] Hippies;

	[Header("Containters")]
	public Transform HippieContainer;
	private float pixelRatioAdjustment;
	private int scrollFrame = 0;

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
		
	}

	void FixedUpdate()
	{
		scrollFrame++;
		scrollFrame = scrollFrame % ScrollFrames;
		if (scrollFrame == 0)
		{
			Vector3 scrollVector = Vector2.right * ScrollRate;
			WorldCamera.transform.position += scrollVector;
		}
	}

	public void SpawnHippie(GameObject enemy)
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
		Destroy(enemy);
	}
}
