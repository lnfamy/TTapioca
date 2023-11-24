using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

	private ScoreText _instance;
	public Text scoreText;
	private GameManager gm;
	private int Score = 0;

	void Awake(){
		_instance = this;
		GameObject.Find ("PauseMenu").GetComponent<Canvas>().enabled = false;
	}

	void Start(){
		gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameManager> ().GetInstance ();
	}

	public ScoreText GetInstance(){
		return _instance;
	}




	public int GetScore ()
	{
		if (this.Score == null) {
			return 0;
		}
		return this.Score;
	}

	public void IncrementScore(){
		this.Score++;
		UpdateScore (this.Score);
	}
		
	public void DecrementScore(){
		this.Score -= this.Score;	
		UpdateScore (this.Score);
	}

	public void ResetScore(){
		UpdateScore (0);
	}

	public string UpdateScore (int newScore)
	{
		string score;
		this.Score = newScore;
		if (this.Score < 10) {
			score = ("0"+this.Score.ToString());
		} else {
			score = (this.Score.ToString());
		}
		scoreText.text = "" + score.ToString ();

		return scoreText.text;
	}

}
