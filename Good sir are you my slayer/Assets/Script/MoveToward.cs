using UnityEngine;
using System.Collections;

public class MoveToward : MonoBehaviour {

	public Vector3 MoveTo;

	void OnTriggerStay(Collider col)
	{
		Player player = col.GetComponent<Player> ();
		if (player != null) 
		{
			print ("yes");
			if (Input.GetButtonDown("X"))
			{
				player.transform.position = MoveTo;
			}
		}
	}
}
