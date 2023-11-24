using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButton : MonoBehaviour
{
	private SpriteRenderer sprRen;
	private GameButton instance;
	private Sprite unpressed, pressed;
	private PlayerMovement pM;
	public GameObject player;
	private Vector3 toDestroy;
	private bool destroyed=false;

	private Grid grid;
	public Vector3 buttonPos;
	public List<Vector3> obstPos = new List<Vector3>();
	private Vector3 ob;
	public List<GameObject> Obstacles = new List<GameObject>();


	void Awake ()
	{
		instance = this;
		sprRen = this.gameObject.GetComponent<SpriteRenderer> ();

		unpressed = Resources.Load<Sprite> ("btn_round_big");
		pressed = Resources.Load<Sprite> ("btn_round_big_hover");

		sprRen.sprite = unpressed;

	
	}



	void Start ()
	{
		this.transform.position = new Vector3 (buttonPos.x, buttonPos.y, buttonPos.z);
		for (int i = 0; i < obstPos.Count; i++) {
			Obstacles [i].transform.position = obstPos [i];
		}	
		pM = player.GetComponent<PlayerMovement> ().GetInstance ();

	}

	public GameButton GetInstance ()
	{
		return instance;
	}

	public void OnTriggerEnter (Collider info)
	{
		if (info.tag == "Player") {
			sprRen.sprite = pressed;
		
			if (!destroyed) {
				foreach(GameObject obs in Obstacles){
					DestroyObstacle (obs);	
				}

			}
		
		}
	}

	//possibly unnecessary V
//	public Vector3 GetObstaclePos(){
//		return this.obstPos;
//	}

	public void DestroyObstacle (GameObject ob)
	{
		

		 toDestroy = new Vector3 (
			ob.transform.position.x,
			ob.transform.position.y,
			ob.transform.position.z
		                    );



		ob.GetComponent<GameObstacle> ().GetInstance ().SetDestroy (true);
		pM.DeleteFromArray (toDestroy);
		destroyed = true;

	}

	public Sprite GetUnpressed(){
		return this.unpressed;
	}
	public void SetDestroyed(bool nwDestroyed){
		this.destroyed = nwDestroyed;
	}

	public void OnResetting(){
		pM = player.GetComponent<PlayerMovement> ().GetInstance ();
	}

}
