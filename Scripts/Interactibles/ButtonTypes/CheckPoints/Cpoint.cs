using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class Cpoint : MonoBehaviour {

	private Cpoint instance;
	private GameObject SpawnP;
	private SpawnPos sp;
	private bool trigger;
	private SpriteRenderer sprren;
	public Vector3 checkpoint;
	private Sprite unpressed,pressed;

	public List<GameObject> wontReset = new List<GameObject>();

	//each gameobject will be one checkpoint
	//contains:
	//	- mesh renderer, mesh filter, sprite
	//	- two sprite variables one is unpressed second is pressed 
	//	- one vector3 variable to store the current checkpoint

	// NEED
	// a way to check whhether player activated second checkpoint
	// -- to do that we will assign checkpoint tags
	// -- if theres a gameobject in the scene called checkpoint1 it will be destroyed

	void Awake(){
		trigger = false;
		instance = this;
		gameObject.transform.position = checkpoint;
		sprren = this.gameObject.GetComponent<SpriteRenderer> ();
		this.gameObject.tag = "Checkpoint";

		unpressed = Resources.Load<Sprite> ("button_set05_b");
		pressed = Resources.Load<Sprite> ("button_set05_a");

		sprren.sprite = unpressed;
		this.gameObject.GetComponent<BoxCollider> ().isTrigger = true;

	}

	void Start(){
		SpawnP = GameObject.FindGameObjectWithTag ("initSpawn");
		sp = SpawnP.GetComponent<SpawnPos> ();		
	}

	void OnTriggerEnter(Collider info){
		if (info.tag == "Player") {
			Trigger ();

		}
	}

	private void Trigger(){
		sprren.sprite = pressed;
		if (sp.GetTrigger()) {
			sp.SetTrigger (false);
		}
		CheckForOtherInstances ();
		trigger = true;
	}

	private List<GameObject> cpoints = new List<GameObject>();

	public void CheckForOtherInstances(){

		foreach (GameObject cpoint in GameObject.FindGameObjectsWithTag("Checkpoint")) {
			if (!cpoint.Equals(this.gameObject)) {
				cpoints.Add (cpoint);
			}
		}

		foreach (GameObject notthisCp in cpoints) {
			notthisCp.GetComponent<Cpoint>().ResetTrigger();
		}
	}

	public bool Pressed(){
		if (sprren.sprite == Resources.Load<Sprite> ("button_set05_a") ) {
			return true;
		}
		return false;
	}

	public void Unpress(){
		sprren.sprite = unpressed;
	}

	public void ResetTrigger(){
		this.trigger = false;
	}

	public bool GetTriggerState(){
		return this.trigger;
	}

	public List<GameObject> GetNonResettable(){
		return this.wontReset;
	}
}
