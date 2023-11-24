using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour {

	PathRequestManager rqManager;
	Grid grid;
	public GameObject player;
	private PlayerMovement pm;

	void Awake(){
		rqManager = GetComponent<PathRequestManager> ();
		grid = GetComponent<Grid> ();
		 pm = player.GetComponent<PlayerMovement> () as PlayerMovement;
	}



	public void StartFindPath(Vector3 startP,Vector3 targetP){
		FindPath (startP,targetP);
	}

	public void FindPath(Vector3 startPos,Vector3 targetPos){

		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		Node startNode = grid.NodeFromWorldPoint (startPos);
		Node targetNode = grid.NodeFromWorldPoint (targetPos);

		if (startNode.walkable && targetNode.walkable) {
			Heap<Node> openSet = new Heap<Node> (grid.MaxSize);
			HashSet<Node> closedSet = new HashSet<Node> ();
			openSet.Add (startNode);

			while (openSet.Count>0) {
				Node currNode = openSet.RemoveFirstItem ();

				closedSet.Add (currNode);

				if (currNode == targetNode) {
					
					pathSuccess = true;
					break; //to exit the loop
				}

				foreach (Node neighbour in grid.GetNeighbours(currNode)) {
					if (!neighbour.walkable || closedSet.Contains(neighbour) ) { //|| !pm.MapLimits(neighbour.worldPosition
						continue;
					}

					int nwMCostToN = currNode.gCost + GetDistance (currNode,neighbour);

					if (nwMCostToN < neighbour.gCost || !openSet.Contains(neighbour)) {
						neighbour.gCost = nwMCostToN;
						neighbour.hCost = GetDistance (neighbour,targetNode);
						neighbour.parent = currNode;

						if (!openSet.Contains(neighbour)) {
							openSet.Add (neighbour);

						} else {
							openSet.UpdateItem (neighbour);
						}
					}
				}
			}
		}
	
		if (pathSuccess) {
			waypoints =	RetraceP (startNode, targetNode);

		}
		rqManager.FinishedProcessingPath (waypoints,pathSuccess);
	}


	private Node currentNode;

	Vector3[] RetraceP(Node startNode,Node endNode){
		List<Node> path = new List<Node>();
		currentNode = endNode;

		while (currentNode != startNode) {
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}
		List<Vector3> waypoints = new List<Vector3> ();
		foreach(Node n in path){
			waypoints.Add (n.worldPosition);
		}
		return waypoints.ToArray ();
	
	}

//	Vector3[] SimplifyPath(List<Node> path) {
//		List<Vector3> waypoints = new List<Vector3>();
//		Vector2 directionOld = Vector2.zero;
//
//		waypoints.Add (path[0].worldPosition);
//		for (int i = 1; i < path.Count; i ++) {
//			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
//			if (directionNew != directionOld) {
//				waypoints.Add(path[i].worldPosition);
//			}
//			directionOld = directionNew;
//		}
//		return waypoints.ToArray();
//	}
	private int dstX,dstY;
	int GetDistance(Node nA,Node nB){
		dstX = Mathf.Abs (nA.gridX - nB.gridX);
		dstY = Mathf.Abs (nA.gridY - nB.gridY);

		if (dstX>dstY) {
			return 14 * dstY + 10 *(dstX-dstY);
		}
		return 14*dstX + 10*(dstY-dstX);
	}
}
