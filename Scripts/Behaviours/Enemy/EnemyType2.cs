using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType2 : EnemyBehaviour
{
	private EnemyType2 _type2instance;
	private int wait = 1;
	private int randomMoveIn = 2;
	private Vector3 playerPs;
	private Vector3[] pathref;
	private int rand;

	public GameObject sprChild;
	public GameObject pla;
	private Vector3[] neighbours = new Vector3[4];


	void Awake ()
	{
		_type2instance = this;
		SetSprChild (sprChild);
		base.Deactivate ();
	}
		

	public void SpawnEnemy2 (GameObject t,Vector3 spawn)
	{
		transform.position = spawn;
		sprend.enabled = true;
		gameObject.GetComponent<BoxCollider> ().enabled = true;
		SetTarget (t);
		OnSpawnPass ();
		anim.enabled = true;
	}

	//every two steps this enemy will take a step in a random direction
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

				if (wait == 0) { //if wait ==0 enemy moves
					if (randomMoveIn > 0) {
						randomMoveIn--;
						wait = 1;
						OneTurn (0);
						return;
					} else {
						randomMoveIn = 2;
						wait = 1;
						MoveInRandomDirection ();
					}
						
					return;
				} else {
					wait--;
				}
			}
		}
	}

	public Vector3[] GetNeighbours ()
	{
		List<Vector3> neighboursCoords = new List<Vector3> ();
		Node currentNode = grid.NodeFromWorldPoint (transform.position);
		List<Node> neighbours = new List<Node> ();
		neighbours = grid.GetNeighbours (currentNode);

		foreach (Node n in neighbours) {
			if (n.walkable) {
				neighboursCoords.Add (n.worldPosition);
			}
		}

		Vector3[] neighRet = neighboursCoords.ToArray ();
		neighboursCoords.Clear ();
		neighboursCoords.Clear ();
		return neighRet;
	}

	public void MoveInRandomDirection ()
	{
		neighbours = new Vector3[4];
		neighbours = GetNeighbours ();
		rand = (int)Random.Range (0, 3);
		while (neighbours[rand] == transform.position) {
			rand = (int)Random.Range (0, 3);

		}
		Vector3 moveTo = new Vector3 (neighbours [rand].x, 0.1f, neighbours [rand].z);
		transform.position = Vector3.MoveTowards (transform.position, moveTo, speed * Time.fixedDeltaTime);
	}


	public override bool GetDead ()
	{
		return base.GetDead ();
	}

	public bool IsWaiting ()
	{
		if (this.wait != 0) {
			return true;
		}
		return false;
	}

	public Node GetPosition ()
	{
		return base.Pos (gameObject.transform.position);
	}

	public override void OnReset ()
	{
		this.wait = 2;
		randomMoveIn = 1;
		base.OnReset ();
	}

	public override bool Activated ()
	{
		return base.Activated ();
	}
}
