using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : IHeapItem<Node> {

	public bool walkable;
	public Vector3 worldPosition;
	public int gCost, hCost; // fcost = gcost + hcost
	public int gridX,gridY;
	public Node parent;
	int heapIndex;

	public Node(bool _walkable, Vector3 _worldPos,int _x,int _y){
	
		this.walkable = _walkable;
		this.worldPosition = _worldPos;
		gridX = _x;
		gridY = _y;
	}

	public bool IsWalkable(){
		return walkable;
	}
	public Vector3 GetWorldPos(){
		return this.worldPosition;
	}


	public int fCost{
		get	{ 
			return gCost + hCost;
		}

	}

	//impleheapIndexHeapItem
	public int HeapIndex {
		get { 
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo(Node nComp){
		int comp = fCost.CompareTo(nComp.fCost); 
		if (comp == 0) {
			
		
			//if comp == 0 that means thheapIndexs are the same
			//in which case we will choose the item with the better hcost
			comp = hCost.CompareTo(nComp.hCost);
		}

		//generally compareto returns 1 if its higher 
		//but since we want the lower one, we'll return -compare 
		//to signify that it's the opposite
		return -comp;
	}
}
