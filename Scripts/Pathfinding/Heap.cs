using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T> {

	T[] items;
	int currItemCount;

	public Heap(int maxHeapSize){
		items = new T[maxHeapSize];
	}

	public void Add(T item){
		item.HeapIndex = currItemCount;
		items [currItemCount] = item;
		SortUp (item);	
		currItemCount++;
	}

	public T RemoveFirstItem(){
		T fItem = items [0];
		currItemCount--;
		items [0] = items [currItemCount];
		items [0].HeapIndex = 0;
		SortDown (items[0]);
		return fItem;
	}


	//returns true if the item in the array with the same index as the passed in item is equal to the passed in item
	public bool Contains(T item){
		return Equals (items[item.HeapIndex],item); 
	}

	public void UpdateItem(T item){
		SortUp (item);
	}

	public int Count{
		get{ 
			return currItemCount;
		}
	}

	void SortDown(T item){
		while (true) {
			int cIndexL = item.HeapIndex * 2 + 1;
			int cIndexR = item.HeapIndex * 2 + 2;
			int swapindex = 0;
			if (cIndexL<currItemCount) {
				swapindex = cIndexL;

				if (cIndexR < currItemCount) {
					if (items[cIndexL].CompareTo(items[cIndexR])<0) {
						//the default assumption is that l has higher priority than r
						//so this statement exists to check if that's not true and act accordingly
						swapindex = cIndexR;
					}
				}

				if (item.CompareTo(items[swapindex])<0) { 
					//if the highest priority child has higher priority than its parent, they will be swapped
					Swap(item,items[swapindex]);
				} else {
					return;
				}
			} else {
				return;
			}

		}
	}

	void SortUp(T item){
		int parentIndex = (item.HeapIndex - 1) / 2;

		while (true) {
			T parentItem = items [parentIndex];
			if (item.CompareTo (parentItem) > 0) { // if item hs higher priority than its parent
				Swap(item,parentItem);
			} else {
				break;
			}
		}
	}

	void Swap(T itemA, T itemB){
		items [itemA.HeapIndex] = itemB;
		items [itemB.HeapIndex] = itemA; 

		int aIndex = itemA.HeapIndex;
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = aIndex;

	}
}

public interface IHeapItem<T> : IComparable<T>{
	int HeapIndex {
		get;
		set;
	}
}