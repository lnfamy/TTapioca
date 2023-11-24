using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

	public Transform target;
	protected float speed = 100f;
	Vector3[] path;
	int targetIndex;
	private Vector3 currentPos;
	protected Animator anim;
	protected SpriteRenderer sprend;
	protected Grid grid;
	private bool above = false, below = false;
	protected bool notDestroyed = true, active = false;
	private GameObject spr;
	private PlayerMovement pm;

	private Vector2 targetNode, enemNode;

	public Vector3 spawnPoint;

	void Awake ()
	{
		transform.position = spawnPoint;

	}

	void Start ()
	{
		grid = FindObjectOfType<Grid> () as Grid;
		pm = target.GetComponent<PlayerMovement> () as PlayerMovement;
	}

	public void SetTarget (GameObject t)
	{
		this.target = t.transform;
	}

	public void OnPlayerMovement ()
	{

		Debug.Log ("Called OnPlayerMovement in UNIT");
		if (active) {
			PathRequestManager.RequestPath 
			(transform.position,
				target.position,
				OnPathFound);		
		}
	
		
	}

	public void SetSprChild (GameObject sprChild)
	{
		this.spr = sprChild;
		sprend = this.spr.GetComponent<SpriteRenderer> () as SpriteRenderer;
		anim = this.spr.GetComponent<Animator> () as Animator;
	}

	public Node Pos (Vector3 tr)
	{
		return grid.NodeFromWorldPoint (tr);
	}

	public void OnPathFound (Vector3[] newPath, bool pathSuccessful)
	{
		if (pathSuccessful) {
			path = newPath;

		}
	}

	public Vector3[] GetPath ()
	{
		return this.path;
	}

	public void OneTurn (int wait)
	{

		Vector3 targPos = target.position;
		if (SideBSide (wait)) {
			Kill ();
			return;
		}


		Vector3 moveTo = path [GetNextIndex (transform.position)];
		transform.position = Vector3.MoveTowards (transform.position, new Vector3 (moveTo.x, 0.1f, moveTo.z), speed * Time.deltaTime);

	}


	public bool SideBSide (int wait)
	{
		Vector2 enemy = grid.CoordsFromWorldPoint (transform.position);
		Vector2 player = grid.CoordsFromWorldPoint (target.transform.position);
		int xP, yP, xE, yE;
		xP = (int)player.x;
		yP = (int)player.y;
		xE = (int)enemy.x;
		yE = (int)enemy.y;
		if (((Mathf.Abs (xE - xP) == 1) && (yE == yP)) || ((Mathf.Abs (yE - yP) == 1) && (xE == xP))) {
			if (wait == 0) {
				return true;
			}
		}
		return false;
	}


	public void FlipSprite (Vector3 playerpos) //checks where player is relative to enemy pos
	{
		targetNode = grid.CoordsFromWorldPoint (playerpos);
		enemNode = grid.CoordsFromWorldPoint (transform.position);

		if ((int)enemNode.y == (int)targetNode.y) {
			Debug.Log ("Flipped sprite");
			sprend.flipX = true;
		}

		if ((int)enemNode.x == (int)targetNode.x) {
			if ((int)enemNode.y > (int)targetNode.y) {
				Debug.Log ("enemoy node above player node");
				above = true;
				below = false;
			} else {
				Debug.Log ("enemy node below player node");
				above = false;
				below = true;
			}
		}
	}



	public void UpdateTargetPos (Vector3 newPos)
	{
		this.target.transform.position = newPos;
	}

	public bool FacingRight ()
	{
		if (sprend.flipX == true) {
			return true;
		}
		return false;
	}

	public void Kill ()
	{
		if (FacingRight ()) {
			anim.Play ("KillPlayerRight");
		} else {
			if (below) {
				anim.Play ("KillPlayerUp");
			} else if (above) {
				anim.Play ("KillPlayerDown");
			} else {
				anim.Play ("KillPlayerLeft");
			}
		}
		pm.Die ();

	}


	public void Die ()
	{
		anim.Play ("EnemyDeath");
		anim.Play ("Disappear");
		sprend.enabled = false;
		anim.enabled = false;
		GetComponent<BoxCollider> ().enabled = false;
		notDestroyed = false;

	}

	public virtual bool GetDead ()
	{
		return !notDestroyed;
	}

	public virtual void OnReset ()
	{
		Deactivate ();
		this.active = false;
		this.notDestroyed = true;
	}

	public Vector3 GetNextPosition ()
	{
		return this.path [GetNextIndex (transform.position)];
	}

	public Vector3 GetTargetPos ()
	{
		return this.target.transform.position;
	}

	public int GetNextIndex (Vector3 lookFor)
	{
		float minDistance = 100f;
		int index = -1;
		for (int i = 0; i < path.Length; i++) {
			
			float absolDist = Mathf.Abs (Vector3.Distance (lookFor, path [i]));

			if (absolDist < minDistance) {
				minDistance = absolDist;
				index = i;
			}
		}
		return index;
	}

	public Vector3 GetSpawnPosition ()
	{
		return this.spawnPoint;
	}

	public void OnSpawnPass(){
		this.active = true;

	}

	public void Deactivate(){
		anim.enabled = false;
		transform.position = spawnPoint;
		sprend.enabled = false;
	}

	public virtual bool Activated(){
		return this.active;
	}
}
