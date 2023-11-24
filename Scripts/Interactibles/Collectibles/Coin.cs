using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, Collectible
{
	private string type = "coin";
	private Vector3 pos;
	public ScoreText txt;
	private Coin inst;
	private GameObject canv;




	void Awake(){
		inst = this;
	}


	public Coin GetInstance(){
		return this.inst;
	}

	public void DecrementOneCoin(){
		txt.DecrementScore ();
	}

	public  void OnTriggerEnter (Collider information)
	{
		if (information.tag == "Player") {
			Debug.Log ("Collected coin");
			txt.IncrementScore ();
			this.DestroyObj ();
			return;
		}
	}

	public void DestroyObj(){
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		gameObject.GetComponent<BoxCollider> ().enabled = false;
	}


}
