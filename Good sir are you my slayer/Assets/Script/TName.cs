using UnityEngine;
using System.Collections;
using Database;

public class TName : MonoBehaviour {

	public Npc npc;
    public SpriteRenderer head;
    public SpriteRenderer body;

	// Use this for initialization
	void Start () 
	{
		Get.TargetName = Get.Name;
		npc.Name = Get.TargetName;

        Get.TargetHead = head.sprite;
        Get.TargetBody = body.sprite;
    }
}
