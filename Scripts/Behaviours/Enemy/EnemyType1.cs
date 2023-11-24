using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyType1 : EnemyBehaviour
{
	private EnemyType1 _type1instance;
	private Vector3 position;
	private int wait = 2;
	private Vector3 playerPs;
	Vector3[] pathref;
	public GameObject sprChild;
	public GameObject pla;

	// add detection radius mechanic

	void Awake ()
	{
		_type1instance = this;
		SetSprChild (sprChild);
		base.Deactivate ();


	}

	public void SpawnEnemy1(GameObject t,Vector3 spawn){
		transform.position = spawn;
		sprend.enabled = true;
		gameObject.GetComponent<BoxCollider> ().enabled = true;
		SetTarget (t);
		OnSpawnPass ();
		anim.enabled = true;
	}

	public void Move ()
	{
		if (!base.active) {
			return;
		}
		OnPlayerMovement ();
		UpdateTargetPos (pla.transform.position);
		pathref = GetPath ();
		if (SideBSide (wait)) {
			Debug.Log (SideBSide (wait));
			Kill ();
			return;
		} else {
			if (notDestroyed) {
				playerPs = GetTargetPos ();
				FlipSprite (playerPs);

				if (wait == 0) {
					OneTurn (wait);	
					wait = 1;
					return;
				} else {
					wait--;
				}
			}
		}

	}

	public override bool Activated ()
	{
		return base.Activated ();
	}

	public override bool GetDead ()
	{
		return base.GetDead ();
	}

	public Node GetPosition ()
	{
		return base.Pos (transform.position);
	}

	public bool IsWaiting ()
	{
		if (this.wait != 0) {
			return true;
		}
		return false;
	}

	public override void OnReset ()
	{
		this.wait = 2;
		base.OnReset ();
	}

}
