using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Collectible 
{
	void OnTriggerEnter (Collider information);
	void DestroyObj ();

}
