using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerParameters : ResetObject
{
	public GameObject player;
	public GameObject throwaway;
	private GameObject spawnPoint;
	public DeathBubble dbubb;
	private List<Vector3> resetLoc;
	private Vector3[] nonwalkable;
	private Vector3 sp;
	public GameObject animChild;
	private Component pMovementOld,pMovementNew;
	private List<GameObject> enemy1,enemy2;



	Component CopyComponent (Component original, GameObject destination)
	{
		System.Type type = original.GetType ();
		Component copy = destination.AddComponent (type);
		// Copied fields can be restricted with BindingFlags
		System.Reflection.FieldInfo[] fields = type.GetFields (); 
		foreach (System.Reflection.FieldInfo field in fields) {
			field.SetValue (copy, field.GetValue (original));
		}
		return copy;
	}
		

	public override void Reset ()
	{
		animChild.GetComponent<Animator>().enabled = false;
		pMovementOld = player.GetComponent<PlayerMovement> ();
		CopyComponent (pMovementOld,throwaway);
		GameObject fireGroupN = player.GetComponent<PlayerMovement> ().fireGroup;
		enemy1 = player.GetComponent<PlayerMovement> ().GetEnemies (1);
		enemy2 = player.GetComponent<PlayerMovement> ().GetEnemies (2);

		PlayerMovement movement = pMovementOld as PlayerMovement;
		DestroyImmediate (player.GetComponent<PlayerMovement>());

		player.transform.position = spawnPoint.transform.position;


		pMovementNew = throwaway.GetComponent<PlayerMovement> () as PlayerMovement;
		player.AddComponent (pMovementNew.GetType());
		player.GetComponent<PlayerMovement> ().speed = 100;
		player.GetComponent<PlayerMovement> ().distance = 1;
		player.GetComponent<PlayerMovement> ().Spawn = spawnPoint.transform;
		player.GetComponent<PlayerMovement> ().Player = player.transform;
		player.GetComponent<PlayerMovement> ().fireGroup = fireGroupN;
		player.GetComponent<PlayerMovement> ().spriteChild = animChild;
		player.GetComponent<PlayerMovement> ().EnemyType1s = enemy1;
		player.GetComponent<PlayerMovement> ().EnemyType2s = enemy2;



		DestroyImmediate (throwaway.GetComponent<PlayerMovement>());
		player.GetComponent<PlayerMovement> ().OnResetting ();
		nonwalkable = player.GetComponent<PlayerMovement> ().GetInstance ().GetUnwalkableForReset ().ToArray ();
		dbubb.GetComponent<SpriteRenderer> ().enabled = false;
		animChild.GetComponent<Animator> ().enabled = true;
		player.GetComponent<PlayerMovement> ().SetNotGameOver (true);
	

	

	}

	public void SetSpawnPoint(GameObject sp){
		this.spawnPoint = sp;
	}

	public string GetSpawnPoint(){
		return this.spawnPoint.name;
	}
}