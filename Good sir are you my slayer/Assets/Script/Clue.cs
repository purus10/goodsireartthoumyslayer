using UnityEngine;
using System.Collections;
using Database;

public class Clue : MonoBehaviour {
	
	// Update is called once per frame
	void OnTriggerStay (Collider col)
    {
        Player player = col.GetComponent<Player>();

        if (player != null)
        {
            if (Input.GetButtonDown("X"))
            {
                if (player.TargetHead.sprite == null)
                    player.TargetHead.sprite = Get.TargetHead;
                else if (player.TargetBody.sprite == null)
                    player.TargetBody.sprite = Get.TargetBody;
                else
                    player.TargetName = Get.TargetName;
                GameObject.Destroy(gameObject);
            }
        }
    }
}
