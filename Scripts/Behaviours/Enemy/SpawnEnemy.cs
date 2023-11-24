using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : ResetObject {
	public GameObject enemy;
	public int enemyType;
	private bool steppedOn = false;
	public Vector3 spawnPoint;
	private GameObject enemChild;
	private SpriteRenderer spr;
	private EnemyType1 enm1;
	private EnemyType2 enm2;

	void OnTriggerEnter(Collider info){
		if (info.tag == "Player" && !steppedOn) {
			if (enemyType == 1) {
				enm1 = enemy.GetComponent<EnemyType1> () as EnemyType1;
				enm1.SpawnEnemy1 (GameObject.FindGameObjectWithTag("Player"),spawnPoint);
			} else {
				enm2 = enemy.GetComponent<EnemyType2> () as EnemyType2;
				enm2.SpawnEnemy2 (GameObject.FindGameObjectWithTag("Player"),spawnPoint);
			}

			steppedOn = true;
		}
	}

	public override void Reset ()
	{
		steppedOn = false;
	}
}
