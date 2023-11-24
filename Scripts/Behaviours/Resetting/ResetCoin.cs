using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCoin : ResetObject {

	public override void Reset ()
	{
		if (gameObject.GetComponent<SpriteRenderer>().enabled==false) {
			this.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
			gameObject.GetComponent<BoxCollider> ().enabled = true;
			gameObject.GetComponent<Coin> ().DecrementOneCoin ();
		}
	
	}

}
