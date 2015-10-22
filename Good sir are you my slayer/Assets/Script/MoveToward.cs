using UnityEngine;
using System.Collections;

public class MoveToward : MonoBehaviour {

	public Vector3 MoveTo;

	void OnTriggerStay(Collider col)
	{
		Player player = col.GetComponentInChildren<Player> ();
		if (player != null) 
		{
			if (Input.GetButtonDown("X"))
			{
				print ("yes");
				player.transform.position = MoveTo;
			}
		}
	}
}
