﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Pathfinding : MonoBehaviour {

	PathRequestManager requestManager;
	Grid grid;
	
	void Awake()
	{
		requestManager = GetComponent<PathRequestManager> ();
		grid = GetComponent<Grid> ();
	}

	public void StartFindPath(Vector3 startpos, Vector3 targetpos)
	{
		StartCoroutine(FindPath(startpos,targetpos));
	}
	
	IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Vector3[] waypoints = new Vector3[0];
		bool pathSuccess = false;

		Node startNode = grid.NodeFromWorlPoint (startPos);
		Node targetNode = grid.NodeFromWorlPoint (targetPos);

		if (startNode.Walkable && targetNode.Walkable) {
			Heap<Node> openSet = new Heap<Node> (grid.MaxSize);
			HashSet<Node> closedSet = new HashSet<Node> ();
			openSet.Add (startNode);

			while (openSet.Count > 0) {
				Node currentNode = openSet.RemoveFirst ();
				closedSet.Add (currentNode);

				if (currentNode == targetNode) {
					pathSuccess = true;
					break;
				}

				foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
					if (!neighbour.Walkable || closedSet.Contains (neighbour))
						continue;
					int newMovementCostToNeighbour = currentNode.gCost + GetDistance (currentNode, neighbour);
					if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)) {
						neighbour.gCost = newMovementCostToNeighbour;
						neighbour.hCost = GetDistance (neighbour, targetNode);
						neighbour.Parent = currentNode;

						if (!openSet.Contains (neighbour)) {
							openSet.Add (neighbour);
						} else 
							openSet.UpdateItem (neighbour);
					}
				}
			}
			yield return null;
			if (pathSuccess)
			{
				waypoints = RetracePath (startNode, targetNode);
			}
			requestManager.FinishedProcessingPath (waypoints, pathSuccess);
		} else
			print("Not Pathable");
		requestManager.FinishedProcessingPath (waypoints, pathSuccess);
		yield break;

	}

	Vector3[] RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node> ();
		Node currentNode = endNode;

		while (currentNode != startNode) 
		{
			path.Add(currentNode);
			currentNode = currentNode.Parent;
		}
		Vector3[] waypoints = SimplifyPath (path);
		Array.Reverse(waypoints);
		return waypoints;
	}

	Vector3[] SimplifyPath(List<Node> path)
	{
		List<Vector3> waypoints = new List<Vector3> ();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i++) 
		{
			Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
				if (directionNew != directionOld)
			{
				waypoints.Add(path[i].WorldPosition);
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}

	int GetDistance(Node nodeA, Node nodeB)
	{
		int distX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs (nodeA.gridY - nodeB.gridY);

		if (distX > distY)
			return 14 * distY + 10 * (distX - distY);
		return 14 * distX + 10 * (distY - distX);
	}
}
