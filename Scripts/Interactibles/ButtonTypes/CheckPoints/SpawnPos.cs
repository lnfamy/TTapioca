using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPos : MonoBehaviour
{
	private bool trigger = false;
	private Vector3 p;
	public GameObject S;

	// Use this for initialization
	void Awake ()
	{
		if (SceneManager.GetActiveScene ().name == "Level01") {
			p = new Vector3 (9.5f, 0.1f, -2.5f);

		} else if (SceneManager.GetActiveScene ().name == "Level02") {
			p = new Vector3 (9.5f,0.1f,-1.5f);
		}

		gameObject.transform.position = p;
		S.transform.position = p;
		trigger = true;
	}

	public void SetTrigger (bool t)
	{
		this.trigger = t;
	}

	public bool GetTrigger ()
	{
		return this.trigger;
	}

}
