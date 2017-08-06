using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour {
	public float Tick = 0.3f;
	public GameObject GameOverPanel;
	public Text GetAJob;
	public Text YourScore;
	public Text HighScore;
	public Text PlayAgain;
	private int state = 0;
	private float nextTick;

	// Use this for initialization
	void Start () {
		nextTick = Time.time + Tick;
		GameOverPanel.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
