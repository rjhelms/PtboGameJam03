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
	public int state = 0;
	private float nextTick;

	// Use this for initialization
	void Start () {
		nextTick = Time.time + Tick;
		GameOverPanel.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if (state < 5)
		{
			if (Time.time >= nextTick)
			{
				nextTick += Tick;
				if (state == 0)
				{
					GetAJob.enabled = true;
					state++;
				} else if (state == 1)
				{
					YourScore.text = "YOUR SCORE\n" + ScoreManager.Instance.Score;
					YourScore.enabled = true;
					state++;
				} else if (state == 2)
				{
					int highScore = 0;
					if(PlayerPrefs.HasKey("highScore"))
					{
						highScore = PlayerPrefs.GetInt("highScore");
					}
					HighScore.text = "HIGH SCORE\n" + highScore;
					HighScore.enabled = true;
					if (ScoreManager.Instance.Score < highScore)
					{
						state += 2;
					} else {
						highScore = ScoreManager.Instance.Score;
						PlayerPrefs.SetInt("highScore", highScore);
						state++;
					}
				} else if (state == 3)
				{
					HighScore.text = "HIGH SCORE\n" + PlayerPrefs.GetInt("highScore");
					state++;
				} else if (state == 4)
				{
					PlayAgain.enabled = true;
					state++;
				}
			}
		} else {
			if (Input.GetButtonDown("Fire1"))
			{
				ScoreManager.Instance.Reset();
				SceneManager.LoadScene(0);
			}
		}
	}
}
