using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node> {

	public bool Walkable;
	public Vector3 WorldPosition;
	public int gridX, gridY;
	int heapIndex;


	public int gCost,hCost;
	public Node Parent;
	public int fCost { get { return gCost + hCost; } }

	public Node(bool _walkable, Vector3 _worldpos, int _gridX, int _gridY)
	{
		Walkable = _walkable;
		WorldPosition = _worldpos;
		gridX = _gridX;
		gridY = _gridY;

	}

	public int HeapIndex
	{
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo(Node nodeToCompare)
	{
		int compare = fCost.CompareTo (nodeToCompare.fCost);
		if (compare == 0) 
		{
			compare = hCost.CompareTo(nodeToCompare.hCost);
		}
		return -compare;
	}

}
