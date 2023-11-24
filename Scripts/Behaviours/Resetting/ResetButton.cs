using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : ResetObject {
	private Sprite unpressed;

	public override void Reset ()
	{
		unpressed = gameObject.GetComponent<GameButton> ().GetUnpressed ();
		gameObject.GetComponent<SpriteRenderer> ().sprite = unpressed;
		gameObject.GetComponent<GameButton> ().SetDestroyed (false);
	}
}
