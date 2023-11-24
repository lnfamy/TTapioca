using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HurtPlayer : MonoBehaviour
{
	private int wait = 0, burn = 3;
	private HurtPlayer instance;
	public GameObject player;
	bool isBurning;


	void Awake ()
	{
		instance = this;
		this.isBurning = true;
		gameObject.SetActive (true);
		burn--;

	
	}


	public HurtPlayer GetInstance ()
	{
		return instance;
	}


	public void OnCharacterMovement ()
	{
		float alphaValue = 1f;
		
		int changeBurnTo = -1;
		int changeWaitTo = -1;
		Color temp = this.gameObject.GetComponentInParent<SpriteRenderer> ().color;

		Color[] tempChildren = new Color[this.gameObject.transform.childCount];

	
		if (isBurning) {
			
			if (this.burn == 0) {
				alphaValue = 0.0f;
				changeBurnTo = 0;
				changeWaitTo = 2;
				this.isBurning = false;

			
			} else if (this.burn == 1) {
				alphaValue = 0.2f;
				changeBurnTo = this.burn - 1;

			} else if (this.burn == 2) {
				alphaValue = 0.6f;
				changeBurnTo = this.burn - 1;

			} else if (this.burn == 3) {
				alphaValue = 1f;
				changeBurnTo = this.burn - 1;
			}
	

		} else {
			

			alphaValue = 0.0f;
			if (this.wait == 0) {
				alphaValue = 1.0f;
				changeWaitTo = 0;
				changeBurnTo = 2;
				this.isBurning = true;
			} 

			if (this.wait > 0) {
				
				changeWaitTo = this.wait - 1;

			}
		}	
		this.burn = changeBurnTo;
		this.wait = changeWaitTo;
//		if (this.wait == 0 && !isBurning) {
//			isBurning = true;
//		}
//		if (this.burn == 0 && isBurning) {
//			isBurning = false;
//		}
//	

		temp.a = alphaValue;
		this.gameObject.GetComponentInParent<SpriteRenderer> ().color = temp;
		for (int i = 0; i < tempChildren.Length; i++) {
			tempChildren [i] = gameObject.transform.GetChild (i).GetComponent<SpriteRenderer> ().color;
		}

		for (int j = 0; j < tempChildren.Length; j++) {
			tempChildren [j].a = alphaValue;
			gameObject.transform.GetChild (j).GetComponent<SpriteRenderer> ().color = tempChildren [j];
		}
	
	}

	public void OnTriggerEnter (Collider info)
	{
		if (info.tag == "Player") {
			if (this.burn > 1 && isBurning) {
				Debug.Log ("player dead");
				player.GetComponent<PlayerMovement> ().GetInstance ().Die ();
			}


		}
		if (info.tag == "Enemy") {
			GameObject.Find (info.name).GetComponent<EnemyType1> ().Die ();
		}
	}

	//	// Update is called once per frame
	//	void Update ()
	//	{
	//
	//	}
}
