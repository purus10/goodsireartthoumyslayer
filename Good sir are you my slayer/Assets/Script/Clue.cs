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
            if (Input.GetKeyDown(KeyCode.V))
            {
                print("GOT IT");
                if (player.TargetHead == null)
                    player.TargetHead.sprite = Get.TargetHead;
                else player.TargetBody.sprite = Get.TargetBody;
            }
        }
    }
}
