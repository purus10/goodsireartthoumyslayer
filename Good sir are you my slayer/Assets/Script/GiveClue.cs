using UnityEngine;
using System.Collections;
using Database;

public class GiveClue : MonoBehaviour 
{
    public SpriteRenderer HUD_TargetHead;
    public SpriteRenderer HUD_TargetBody;
    public SpriteRenderer HUD_TargetString;



	void OnTriggerStay(Collider col)
	{
		Player player = col.gameObject.GetComponentInParent<Player> ();

		if (player != null) 
		{
            if (Input.GetButtonDown("Y"))
            {
                HUD_TargetHead.sprite = Get.TargetHead;
            }

		}
	}
}
