using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
	private ResetManager instance;
	private  ResetObject[] _resetScripts;
	private ResetObject[] resetscrobj;
	private GameObject[] dr;
	public GameObject player;

	private List<GameObject> dontReset;
	private bool spawn0=false;
	private GameManager gm;
	private string spawnIndex;
	ResetPlayerParameters rp;
	private bool allDiff=true;
	//public List<GameObject> ls = new List<GameObject> ();

	void Awake(){
		instance = this;
	}

	public void ResetScene ()
	{
		rp = player.GetComponent<ResetPlayerParameters> () as ResetPlayerParameters;
		Debug.Log ("Is rp null? = "+(rp==null));
		spawnIndex = rp.GetSpawnPoint ();
		dontReset = new List<GameObject> ();

		if (spawnIndex == "SpawnPos") {
			spawn0 = true;
			dontReset.Add (new GameObject());
		}
		else if (spawnIndex == "Checkpoint1") {
			
			dontReset.AddRange (GameObject.Find("Checkpoint1").GetComponent<Cpoint>().GetNonResettable());
		}
		else if (spawnIndex == "Checkpoint2") {
			dontReset.AddRange (GameObject.Find("Checkpoint2").GetComponent<Cpoint>().GetNonResettable());
		}
		dr = dontReset.ToArray ();


		resetscrobj = FindObjectsOfType (typeof(ResetObject)) as ResetObject[];

	
		for (int i = 0; i < resetscrobj.Length; i++) {
			allDiff = true;
			for (int j = 0; j < dr.Length; j++) {
				if (resetscrobj[i].gameObject.name == dr[j].name ) {
					allDiff = false;
					break;
				}
			}
			if (allDiff) {
				resetscrobj [i].Reset ();
			}
		}


		for (int i = 0; i < _resetScripts.Length; i++) {
			if (resetscrobj[i].gameObject.tag=="Button") {
				resetscrobj [i].gameObject.GetComponent<GameButton> ().OnResetting ();
		
				if (resetscrobj[i].gameObject.tag == "Player") {
					resetscrobj [i].gameObject.GetComponent<PlayerMovement> ().OnResetting ();
				}
			}
		}


	}

	public ResetManager GetInstance(){
		return instance;
	}


}
