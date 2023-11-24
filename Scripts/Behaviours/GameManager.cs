using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{

	bool gameEnded = false, playerDead = false, win = false;
	private GameManager instance;

	public Transform gameEndUI;
	public Text endRunScoreTxt;
	private ScoreText sctx2;
	public Image gameOver;

	int highScore = 0;
	int endRunScore, currentRunScore = 0;
	public Text highestSc;
	private GameObject score;
	private ScoreText scoreT;
	private int scorTxt;
	private bool Menu;
	public GameObject newBest, gmOver, lvlcomplete, lvlcompoverlay, compRe, cpRestart;


	private Image GameOver,newbst;
	private Text LevelComplete, lvlcmpOverlay;
	private Button cmpRe, cpRe;
	private Image _comp, _checkp;
	public GameObject player;
	private ResetPlayerParameters rstpp;
	public GameObject cpoint1, cpoint2, initspawn;
	private ResetManager resetMngr;

	private MainLoopable main;

	void Awake ()
	{
		instance = this;

		Menu = SceneManager.GetActiveScene ().name == "MainMenu";



	
	}

	public GameManager GetInstance ()
	{
		return this.instance;
	}


	void Start ()
	{
		if (!Menu) {

			if (SceneManager.GetActiveScene().name == "Level01") {
				FindObjectOfType<TileMap> ().BuildMesh (1); 
				FindObjectOfType<TileMap> ().BuildMesh (1); 

			} else if (SceneManager.GetActiveScene().name == "Level02"){
				FindObjectOfType<TileMap> ().BuildMesh (2); 
				FindObjectOfType<TileMap> ().BuildMesh (2); 


			}
			resetMngr = FindObjectOfType (typeof(ResetManager)) as ResetManager;
		
			score = GameObject.Find ("ScoreText");
			scoreT = score.GetComponent<ScoreText> ();
			rstpp = player.GetComponent<ResetPlayerParameters> () as ResetPlayerParameters;
		

			GameOver = gmOver.GetComponent<Image> () as Image;
			newbst = newBest.GetComponent<Image> () as Image;

			LevelComplete = lvlcomplete.GetComponent<Text> () as Text;
			lvlcmpOverlay = lvlcompoverlay.GetComponent<Text> () as Text;

			cmpRe = compRe.GetComponent<Button> () as Button;
			cpRe = cpRestart.GetComponent<Button> () as Button;

			_comp = compRe.GetComponent<Image> () as Image;
			_checkp = cpRestart.GetComponent<Image> () as Image;
		}
	}


	public float restartDelay = 1f;

	public void EndGame (int indicator) // 1 = win, 2 = player died, 3 = quit
	{	player.GetComponent<PlayerMovement>().SetNotGameOver(false);
		
		scorTxt = scoreT.GetScore ();		
		ScoreSaver (true);
		string txt = scoreT.UpdateScore (endRunScore);
		endRunScoreTxt.text = txt;
		Debug.Log ("End run score: " + endRunScore + " Current run score: " + currentRunScore);
		highestSc.text = this.highScore.ToString ();
		StartCoroutine (waiter ());


		if (highScore > this.endRunScore) {
			newbst.enabled = false;	
		}	
		switch (indicator) {
		case 1:
			Debug.Log ("Win, level complete");
			cpRe.enabled = false;
			LevelComplete.enabled = true;
			lvlcmpOverlay.enabled = true;
			cmpRe.enabled = true;
			_checkp.enabled = false;
			_comp.enabled = true;
			GameOver.enabled = false;
			break;
		case 2:
			Debug.Log ("Player died");
			LevelComplete.enabled = false;
			lvlcmpOverlay.enabled = false;
			GameOver.enabled = true;
			cmpRe.enabled = false;
			cpRe.enabled = true;
			_comp.enabled = false;
			_checkp.enabled = true;
			break;
		default:
			Debug.Log ("Player quit");
			LevelComplete.enabled = false;
			lvlcmpOverlay.enabled = false;
			cmpRe.enabled = true;
			cpRe.enabled = false;
			_comp.enabled = true;
			_checkp.enabled = false;
			break;
			

		}
		scoreT.ResetScore ();
		endRunScoreTxt.GetComponent<ScoreText> ().GetInstance ().ResetScore ();
		endRunScore = 0;
		Time.timeScale = 0f;
	}

	IEnumerator waiter ()
	{
		yield return new WaitForSecondsRealtime (1);

		gameEndUI.GetComponent<Canvas> ().enabled = true;
	}

	public void QuitToMenu ()
	{
		
		this.gameEndUI.gameObject.SetActive (false);
		EndGame (3);
	}

	public void CompleteRestart ()
	{
		rstpp.SetSpawnPoint (initspawn);
		resetMngr.ResetScene ();
		Time.timeScale = 1f;
	}

	public void Restart ()
	{
		Debug.Log ("initspawn trigger: "+initspawn.GetComponent<SpawnPos>().GetTrigger());
		Debug.Log ("cp1 trigger: "+cpoint1.GetComponent<Cpoint> ().GetTriggerState ());
		Debug.Log ("cp2 trigger: "+cpoint2.GetComponent<Cpoint> ().GetTriggerState ());
		if (initspawn.GetComponent<SpawnPos> ().GetTrigger ()) {
			rstpp.SetSpawnPoint (initspawn);
			Debug.Log ("initSpawn");
		} else if (cpoint1.GetComponent<Cpoint> ().GetTriggerState ()) {
			rstpp.SetSpawnPoint (cpoint1);
			Debug.Log ("1st cpoint");
		} else if (cpoint2.GetComponent<Cpoint> ().GetTriggerState ()) {
			rstpp.SetSpawnPoint (cpoint2);
			Debug.Log ("2nd cpoint");
		}
		Debug.Log ("spawnpoint is " + rstpp.GetSpawnPoint ());

		resetMngr.ResetScene ();
		Time.timeScale = 1f;
	}

	public string GetSpIndex ()
	{
		return rstpp.GetSpawnPoint ();
	}

	public void ResumeGame ()
	{
		Time.timeScale = 1f;
	}

	public void ScoreSaver (bool hasGameEnded)
	{
		
		currentRunScore = scorTxt;
		if (currentRunScore > highScore) {
			this.highScore = currentRunScore;	
		}
		if (hasGameEnded) {
			endRunScore = currentRunScore;
		}
	
	}


}