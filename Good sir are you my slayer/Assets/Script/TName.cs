using UnityEngine;
using System.Collections;
using Database;

public class TName : MonoBehaviour {

	public Npc npc;

	// Use this for initialization
	void Awake () 
	{
		Get.TargetName = Get.Name;
		npc.Name = Get.TargetName;
	}
}
