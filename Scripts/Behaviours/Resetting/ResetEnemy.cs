using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEnemy : ResetObject {
	private Vector3 SpawnPos;
	private BoxCollider bxCol;
	private EnemyType1 scType1;
	private EnemyType2 scType2;
	public int type;

	public override void Reset ()
	{
		if (type == 1) {
			scType1 = gameObject.GetComponent<EnemyType1> () as EnemyType1;
			scType1.enabled = true;
			SpawnPos = scType1.GetSpawnPosition ();
			scType1.OnReset ();
		} else {
			scType2 = gameObject.GetComponent<EnemyType2> () as EnemyType2;
			scType2.enabled = true;
			SpawnPos= scType2.GetSpawnPosition ();
			scType2.OnReset ();
			
		}
		transform.position = SpawnPos;
		bxCol = GetComponent<BoxCollider> () as BoxCollider;
		bxCol.enabled = true;

	}

}
