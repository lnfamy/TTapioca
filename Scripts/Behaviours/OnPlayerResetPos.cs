using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerResetPos : MonoBehaviour {
	private bool spawned = false;

	public void ChangeSpawned(bool newspawned){
		spawned = newspawned;
	}

	public void ChangePos(Vector3 newpos){
		if (!this.spawned) {
			gameObject.transform.position = newpos;	
			spawned = true;
		}

	}
}
