using UnityEngine;
using System.Collections;
using Database;

public class GiveClue : MonoBehaviour 
{
	public Sprite Clue;
	public int SearchLength;
	int i = 0;


	void OnTriggerStay(Collision col)
	{
		Player player = col.gameObject.GetComponentInParent<Player> ();

		if (player != null) 
		{
			if (Input.GetKey(KeyCode.L))
			{
				i++;
				if (i >= SearchLength)
					if (Clue == Get.TargetHead)
						player.TargetHead = Clue;
				else if (Clue == Get.TargetBody)
					player.TargetBody = Clue;
			}

		}
	}
}
