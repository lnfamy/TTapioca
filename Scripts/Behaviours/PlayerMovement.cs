using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

	public float distance;
	public float speed;
	private Vector3 sFingerPos;
	private Vector3 lFingerPos;
	private float minDragDistance;
	public Transform Spawn;
	public Transform Player;
	private GameObject aStar;
	private PlayerMovement instance;
	public GameObject fireGroup;
	public GameObject spriteChild;
	private SpriteRenderer sprrend;
	private HurtPlayer fire;

	private bool notGameOver = true;

	bool canMove = false;
	bool hasMoved = false;
	private bool alive = true;
	private Vector3 endPos;
	private Grid grid;
	private DeathBubble bubb;
	private GameManager gm;
	GameObject[] arUnwalkable;
	List<Vector3> unwalkablePos = new List<Vector3> ();
	private Vector3 currLPos;
	private Vector3[] posArr;

	public List<GameObject> EnemyType1s = new List<GameObject> ();
	public List<GameObject> EnemyType2s = new List<GameObject> ();
	private List<EnemyType2> enemy2Units = new List<EnemyType2> ();
	private List<EnemyType1> enemy1Units = new List<EnemyType1> ();


	public void Die ()
	{
		bubb.OnPlayerDeath ();
		alive = false;
		gm.EndGame (2);

	}

	void Awake ()
	{
		instance = this;

	}

	public List<Vector3> GetUnwalkableForReset ()
	{
		return this.unwalkablePos;
	}

	public void SetNotGameOver (bool isit)
	{
		notGameOver = isit;
	}

	void Start ()
	{
		Player.transform.position = Spawn.transform.position;

		fire = fireGroup.GetComponentInParent<HurtPlayer> ().GetInstance ();

		minDragDistance = Screen.height * 15 / 100; //15% of screen height
		endPos = transform.position;
		aStar = GameObject.FindGameObjectWithTag ("aStar"); 
		grid = aStar.GetComponent<Grid> ().GetInstance ();
		bubb = GameObject.FindObjectOfType<DeathBubble> ().GetInstance ();


		arUnwalkable = GameObject.FindGameObjectsWithTag ("Unwalkable");

		for (int i = 0; i < arUnwalkable.Length; i++) {
			currLPos = new Vector3 (arUnwalkable [i].transform.position.x,
				arUnwalkable [i].transform.position.y,
				arUnwalkable [i].transform.position.z);
			unwalkablePos.Add (currLPos);
		}
		posArr = unwalkablePos.ToArray ();

		gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameManager> ().GetInstance ();

		sprrend = GetComponentInChildren<SpriteRenderer> () as SpriteRenderer;

		InitEnemies ();
	}

	public void OnResetting ()
	{
		if (instance == null) {
			instance = this;

		}
		aStar = GameObject.FindGameObjectWithTag ("aStar"); 
		grid = aStar.GetComponent<Grid> ().GetInstance ();
		InitEnemies ();
	}

	public void InitEnemies(){
		foreach (GameObject enem in EnemyType1s) {
			enemy1Units.Add (enem.GetComponent<EnemyType1> () as EnemyType1);
		}
		foreach (GameObject enemy2 in EnemyType2s) {
			enemy2Units.Add (enemy2.GetComponent<EnemyType2> () as EnemyType2);
		}
	}

	public bool MapLimits (Vector3 endP)
	{
		Node n = grid.NodeFromWorldPoint (endP);
		Node n2;
		if (n.walkable == true && !NextPosEnemy1 (n) && !NextPosEnemy2(n)) {

			for (int i = 0; i < posArr.Length; i++) {
				n2 = grid.NodeFromWorldPoint (posArr [i]);
				if (n == n2) {
					return false;
				}
			}

			return true;
		}

		return false;
	}

	public List<GameObject> GetEnemies(int type){
		if (type==1) {
			return EnemyType1s;
		}
		return EnemyType2s;
	}

	public bool NextPosEnemy2 (Node endP)
	{
		foreach (EnemyType2 enemy in enemy2Units) {
			if (enemy.GetPosition () == endP && !enemy.GetDead () && enemy.Activated()) {
				enemy.Die ();
				return true;
			}
		}
		return false;
	}

	public bool NextPosEnemy1 (Node endp)
	{
		foreach (EnemyType1 en in enemy1Units) {
			if (en.GetPosition () == endp && !en.GetDead () && en.Activated()) {
				en.Die ();
				return true;
			}
		}
		return false;
	}


	public void DeleteFromArray (Vector3 toDel)
	{
		List<Vector3> list = new List<Vector3> ();
		Node toodles = grid.NodeFromWorldPoint (toDel);
		Debug.Log ("Entered function");
		for (int i = 0; i < this.posArr.Length; i++) {
			Node n = grid.NodeFromWorldPoint (posArr [i]);
			if (n == toodles) {
				if (!MapLimits (posArr [i])) {
					toodles.walkable = true;
				}
			} else {
				list.Add (posArr [i]);
			}
		}

		this.posArr = new Vector3[list.Count];
		this.posArr = list.ToArray ();

		Debug.Log ("Return");
		return;
	}

	public void CallEnemyMovement ()
	{
		foreach (EnemyType1 type1 in enemy1Units) {
			if (!type1.GetDead()) {
				type1.Move ();
			}

		}
		foreach(EnemyType2 type2 in enemy2Units){
			if (!type2.GetDead()) {
				type2.Move ();
			}

		}

	}

	public PlayerMovement GetInstance ()
	{
		return instance;
	}

	void Update ()
	{
		if (notGameOver) {
			if (Input.touchCount == 1) { //touching screen with one finger
				Touch touch = Input.GetTouch (0);
				if (touch.phase == TouchPhase.Began) {
					sFingerPos = touch.position;
					lFingerPos = touch.position;
				} else if (touch.phase == TouchPhase.Moved) { //update endPos based on where the finger moved
					lFingerPos = touch.position;			
				} else if (touch.phase == TouchPhase.Ended) { //checks if player removed finger from screen
					lFingerPos = touch.position;	

					//checking if minDragDistance>20% of screen height
					if (Mathf.Abs (lFingerPos.x - sFingerPos.x) > minDragDistance || Mathf.Abs (lFingerPos.y - sFingerPos.y) > minDragDistance) {
						//finger was dragged
						if (Mathf.Abs (lFingerPos.x - sFingerPos.x) > Mathf.Abs (lFingerPos.y - sFingerPos.y)) {
							//horizontal movement > vertical movement == player wanted to move horizontally
							if (lFingerPos.x > sFingerPos.x) { // meaning movement is to the right 
								//		Debug.Log ("right swipe");
								endPos = new Vector3 (endPos.x + distance, endPos.y, endPos.z);
							} else if (sFingerPos.x > lFingerPos.x) {
								//		Debug.Log ("left swipe");
								endPos = new Vector3 (endPos.x - distance, endPos.y, endPos.z);

							}

							//player wanted to move vertically
						} else if (lFingerPos.y > sFingerPos.y) { // meaning movement upwards
							///	Debug.Log ("upwards swipe");
							endPos = new Vector3 (endPos.x, endPos.y, endPos.z + distance);
						} else if (sFingerPos.y > lFingerPos.y) {
							//	Debug.Log ("downwards swipe");
							endPos = new Vector3 (endPos.x, endPos.y, endPos.z - distance);
						}
						//transform.position = Vector3.MoveTowards (transform.position, endPos, speed * Time.fixedDeltaTime);	

					} else {
						//drag distance is not enough to be a swipe, therefore it is a tap
						Debug.Log ("tap");
					}
				}
			} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {

				endPos = new Vector3 (endPos.x - distance, endPos.y, endPos.z);

			} else if (Input.GetKeyDown (KeyCode.RightArrow)) {

				endPos = new Vector3 (endPos.x + distance, endPos.y, endPos.z);
			} else if (Input.GetKeyDown (KeyCode.UpArrow)) {

				endPos = new Vector3 (endPos.x, endPos.y, endPos.z + distance);
			} else if (Input.GetKeyDown (KeyCode.DownArrow)) {

				endPos = new Vector3 (endPos.x, endPos.y, endPos.z - distance);
			}

			canMove = MapLimits (endPos);
			if (!canMove) {
				endPos = transform.position;
				transform.position = Vector3.MoveTowards (transform.position, endPos, speed * Time.fixedDeltaTime);
				if (alive) {
					CallEnemyMovement ();
				}


			} else {
				if (endPos != transform.position) {
					if (endPos.x > transform.position.x) {
						sprrend.flipX = false;
					} else if (endPos.x < transform.position.x) {
						sprrend.flipX = true;
					}

					fire.OnCharacterMovement ();
					bubb.FollowPlayer ();

					if (alive) {
						transform.position = Vector3.MoveTowards (transform.position, endPos, speed * Time.fixedDeltaTime);
						CallEnemyMovement ();
					}
				}
				canMove = false;

			} 

			// every time the player moves call PlayerState
		}
	}

		
}
