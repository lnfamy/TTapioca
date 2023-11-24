using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasReset : ResetObject {
	public GameObject PauseUI, gameOverUI;

	public override void Reset ()
	{
		PauseUI.GetComponent<Canvas> ().enabled = false;
		gameOverUI.GetComponent<Canvas> ().enabled = false;
	
	
	}
}
