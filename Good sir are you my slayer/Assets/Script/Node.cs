using UnityEngine;
using System.Collections;

public class Node {

	public bool Walkable;
	public Vector3 WorldPosition;

	public Node(bool _walkable, Vector3 _worldpos)
	{
		Walkable = _walkable;
		WorldPosition = _worldpos;
	}
}
