using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCheckpoint : ResetObject {
	//resetting a checkpoint is...
	//if its sprite is pressed, set to unpressed
	//need to make a bool method to return whether sprite is unpressed or pressed in cpoint
	// and set trigger to false, since a complete restart would mean loss of checkpoints.

	public override void Reset ()
	{
		if (gameObject.GetComponent<Cpoint>().Pressed()) {
			gameObject.GetComponent<Cpoint> ().Unpress ();
		}	
		if (gameObject.GetComponent<Cpoint>().GetTriggerState()) {
			gameObject.GetComponent<Cpoint> ().ResetTrigger ();
		}
	}
}
