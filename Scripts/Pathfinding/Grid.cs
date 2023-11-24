using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Grid : MonoBehaviour
{
	public Transform Tester;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	private Vector3[,] worldP;
	private Vector3 worldBottomLeft, transPos, worldTopLeft;
	public bool displayGridGizmos;

	//default
	Node[,] grid;
	int gridX = 18, gridY = 24;
	private static Grid _instance;

	private float nodeDiameter;





	public void Awake ()
	{
		_instance = this;
		Debug.Log ("Grid instance not null");
		worldBottomLeft = new Vector3 ();
		worldTopLeft = new Vector3 ();
		transPos = transform.position;
		nodeRadius = 0.5f;
		nodeDiameter = nodeRadius * 2;
		worldBottomLeft = transPos - Vector3.right * 18 / 2 - Vector3.forward * 24 / 2;
		worldTopLeft = transPos - Vector3.right * 18 / 2 + Vector3.forward * 24 / 2;
		worldP = new Vector3[18, 24];

		CreateGrid ();

	}

	public Grid GetInstance ()
	{
		return _instance;
	}

	public Node[,] GetNodeArray ()
	{
		return this.grid;
	}

	public int GetGridSizeX ()
	{
		return this.gridX;
	}

	public int GetGridSizeY ()
	{
		return this.gridY;
	}

	public Node NodeFromWorldPoint (Vector3 worldP)
	{
		//Debug.Log (string.Format("DEBUG:{0} worldP.x = {1}{0}transform.position.x = {3}{0}worldP.z = {2}{0}transform.position.z = {4}", System.Environment.NewLine, worldP.x, worldP.z, transform.position.x, transform.position.z));
		float percentX = (worldP.x - transform.position.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldP.z - transform.position.z + gridY / 2) / gridWorldSize.y;
		percentY = Mathf.Clamp01 (percentY);
		if (percentY < 0) {
			percentY = percentY * -1;
		} 
		percentX = Mathf.Clamp01 (percentX);
		int x = Mathf.FloorToInt ((gridX) * percentX);
		int y = Mathf.FloorToInt ((gridY) * percentY);
		 
		return grid [x, gridY - y - 1];
	}

	public Vector2 CoordsFromWorldPoint (Vector3 worldP)
	{
		//Debug.Log (string.Format("DEBUG:{0} worldP.x = {1}{0}transform.position.x = {3}{0}worldP.z = {2}{0}transform.position.z = {4}", System.Environment.NewLine, worldP.x, worldP.z, transform.position.x, transform.position.z));
		float percentX = (worldP.x - transform.position.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldP.z - transform.position.z + gridY / 2) / gridWorldSize.y;

		if (percentY < 0) {
			percentY = Mathf.Clamp (percentY, -1.0f, 0.0f);
		} else {
			percentY = Mathf.Clamp01 (percentY);
		}
		percentX = Mathf.Clamp01 (percentX);
		int x = Mathf.FloorToInt ((gridX) * percentX);
		int y = Mathf.FloorToInt ((gridY) * percentY);

		Debug.Log ("y:" + y);
		return new Vector2 (x, gridY - y - 1);
	}

	void CreateGrid ()
	{
		grid = new Node[gridX, gridY];

		//loop through all the positions of the nodes to do collision checks for walkable/unwalable
		for (int x = 0; x < gridX; x++) {
			for (int y = 0; y < gridY; y++) {
				worldP [x, y] = worldTopLeft + Vector3.right * (x * nodeDiameter + nodeRadius) - Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = false;

				this.grid [x, y] = new Node (walkable, worldP [x, y], x, y);
			}
		}
		//Debug.Log ("Done Grid");
	}

	public int MaxSize{
		get{
			return gridX*gridY;
		}
	}

	public void ChangeWalkable (int iX, int iY, bool walkable)
	{
		if (walkable && !this.grid [iX, iY].walkable) {
			this.grid [iX, iY].walkable = true;
		} else if (!walkable && this.grid [iX, iY].walkable) {
			this.grid [iX, iY].walkable = true;
		}
	}

	private int checkX, checkY;

	public List<Node> GetNeighbours (Node node)
	{
		List<Node> neighbours = new List<Node> (); 
		// i is x, j is y
		for (int i = -1; i <= 1; i++) {
			for (int j = -1; j <= 1; j++) {
				if (i != 0 && j != 0) {
					continue;
				}
				checkX = node.gridX + i;
				checkY = node.gridY + j;

				if (checkX >= 0 && checkX < this.gridX && checkY >= 0 && checkY < this.gridY) {
					neighbours.Add (grid[checkX,checkY]);
				}
			}
		}
		return neighbours;
	}

	void OnDrawGizmos ()
	{
		
		Gizmos.DrawWireCube (transPos, new Vector3 ((float)gridX, 1f, (float)gridY));
		if (grid != null && displayGridGizmos ) {
			Node testNode = NodeFromWorldPoint (Tester.position);
			foreach (Node n in grid) {
				//if its walkable itll be white, otherwise red.
				if (n.walkable) {
					Gizmos.color = Color.white;
				} else {
					Gizmos.color = Color.red;
				}
				if (testNode == n) {
					Gizmos.color = Color.cyan;
				}

			

				Gizmos.DrawCube (n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
			}
		}
	}
}