using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObstacles : ResetObject {

	public override void Reset(){
		this.gameObject.GetComponent<MeshRenderer> ().enabled = true;
		this.gameObject.GetComponent<GameObstacle> ().SetDestroy (false);
	}
}
