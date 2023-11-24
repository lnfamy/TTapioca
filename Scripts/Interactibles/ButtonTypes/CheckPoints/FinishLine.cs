using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
	private Vector3 finishLine;
	private FinishLine instance;
	public GameObject gm, F;
	private GameManager gameManager;


	void Awake ()
	{
		if (SceneManager.GetActiveScene ().name == "Level02") {
			finishLine = new Vector3 (4.5f, 0.1f, -19.5f);
		} else {
			finishLine = new Vector3 (4.5f,0.1f,-18.5f);
		}
		gameObject.transform.position = finishLine;
		F.transform.position = finishLine;
		instance = this;
	}

	void Start ()
	{
		gameManager = gm.GetComponent<GameManager> ();

	}

	public FinishLine GetInstance ()
	{
		return this.instance;
	}

	public void OnTriggerEnter (Collider info)
	{
		
		if (info.tag == "Player") {
			Debug.Log ("Level won");
			gameManager.EndGame (1);

		}
	

	}
}
