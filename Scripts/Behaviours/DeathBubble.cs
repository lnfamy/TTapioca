using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBubble : MonoBehaviour {
	private bool HasDied = false;
	private bool visible = false;
	public GameObject player;
	private SpriteRenderer rend;
	private DeathBubble bubbleInstance;

	public Transform target;
	public Vector3 offset;
	private Vector3 MoveTo;
	private Vector3 curPos;

	void Awake(){
		bubbleInstance = this;
		rend = this.gameObject.GetComponent<SpriteRenderer> ();
		rend.enabled = false;

	}

	public DeathBubble GetInstance(){
		return this.bubbleInstance;
	}

	public void OnPlayerDeath(){
		player.GetComponentInChildren<Animator> ().enabled = false;
		rend.enabled = true;
		visible = true;
		Debug.Log ("GAME OVER");
		//get game to send to game over screen
	}

	public void FollowPlayer(){
		this.curPos = new Vector3 (transform.position.x,transform.position.y,transform.position.z);

		MoveTo = new Vector3 (
			target.position.x + offset.x,
			target.position.y,
			target.position.z + offset.z
		
		);

		transform.position = MoveTo;
	}

}
