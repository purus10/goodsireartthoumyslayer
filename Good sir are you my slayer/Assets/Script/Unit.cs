using UnityEngine;
using System.Collections;
using System;

public class Unit : MonoBehaviour {

	public float MinSpeed;
	public float MaxSpeed;
	public float speed;
	public Vector3[] path;

	public void MoveTo(Vector3 target)
	{
		PathRequestManager.RequestPath (transform.position, target, OnPathFound);
	}

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
	{
		if (pathSuccessful) 
		{
			speed = UnityEngine.Random.Range(0.5f,1.5f);
			path = newPath;
			if (this != null)
			{
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
			}
		}
	}

	IEnumerator FollowPath()
	{
		if (path.Length == 0)
		{
			yield break;
		}
		Vector3 currentWaypoint = path [0];
		int targetIndex = 0;
		while (true) 
		{
			if (transform.position == currentWaypoint)
			{
				targetIndex++;
				if (targetIndex >= path.Length)
				{
					yield break;
				}
				currentWaypoint = path[targetIndex];
			}
			transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
			yield return null;
		}
	}
}
