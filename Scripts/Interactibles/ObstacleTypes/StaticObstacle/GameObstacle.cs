using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObstacle : MonoBehaviour
{
	
	private GameObstacle instance;
	private bool destroy=false;

	void Awake(){
		instance = this;
	}

	public void SetDestroy(bool d){
		this.destroy = d;
	}

	public GameObstacle GetInstance ()
	{
		return instance;
	}

	void Update () {
		if (this.destroy) {
			gameObject.GetComponent<MeshRenderer> ().enabled = false; 

		}
	}

}
