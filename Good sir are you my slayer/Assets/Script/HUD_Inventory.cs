using UnityEngine;
using System.Collections;
using Database;

public class HUD_Inventory : MonoBehaviour {

	public Sprite[] Anim_Highlight;
	public GameObject[] Icons;
	public GameObject[] Inventory;

	void Awake()
	{
		transform.position = Inventory[0].transform.position;
	}
	void Update () 
	{
		if (Get.player != null)
		{
			for (int i = 0;i < Inventory.Length;i++)
			{
				if(Get.player.Slots[i] != null)
				{
					Sprite icon = Get.player.Slots[i].GetComponent<SpriteRenderer>().sprite;
					Icons[i].GetComponent<SpriteRenderer>().sprite = icon;
				}
			}
			
			for (int i = 0; i < Inventory.Length;i++)
			{
				if(Get.player.Selected == Get.player.Slots[i])
				transform.position = Inventory[i].transform.position;
			}
		}


			
	}
}
