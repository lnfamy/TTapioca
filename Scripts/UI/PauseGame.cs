using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour {

	public Transform PauseUI;
	private bool paused=false;
	private GameObject go;
	private GameManager gm;
	public GameObject gameoverUI,pausedUI;

	void Awake(){
		gameoverUI.GetComponent<Canvas> ().enabled = false;
		pausedUI.GetComponent<Canvas> ().enabled = false;
		go = GameObject.Find ("GameManager");
		gm = go.GetComponent<GameManager> ().GetInstance ();
	}

	public void ResumeGame()
	{
		
		PauseUI.gameObject.GetComponent<Canvas> ().enabled = false;
		Time.timeScale = 1f;
	}



	public void QuitToMenu()
	{
		Debug.Log ("Player quit");
		SceneManager.LoadScene ("MainMenu");
		Time.timeScale = 1f;
	}

	public void OpenSettings(){
		PauseUI.GetComponentInChildren<SettingsMenu> (true).gameObject.SetActive (true);
	}

	public void Pause()
	{
		PauseUI.gameObject.GetComponent<Canvas> ().enabled = true;
		PauseUI.gameObject.GetComponentInChildren<SettingsMenu> (true).EnableDisable2 (false); // maybe it will throw an exception if i search for it and it's not active and i try and access its methods SO UH
		paused = true;
		Time.timeScale = 0f;

	}

	public void Unhide(){
		PauseUI.gameObject.GetComponent<Canvas> ().enabled = false;
	}



}
