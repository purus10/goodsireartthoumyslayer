using UnityEngine;
using System.Collections;
using Database;

public class HUD_Inventory : MonoBehaviour {

	public Sprite[] Anim_Highlight;
	public GameObject[] Icons;
	public GameObject[] Inventory;
	public float Anim_speed;
	public Player player;
	//static public bool ;
	float time;
	int frame;

	void Awake()
	{
		transform.position = Inventory[0].transform.position;
	}
	void Update () 
	{
		if (player != null)
		{
			for (int i = 0;i < 2;i++)
			{
                if (player.Slots[i] != null)
                {
                    Sprite icon = player.Slots[i].GetComponent<SpriteRenderer>().sprite;
                    Icons[i].GetComponent<SpriteRenderer>().sprite = icon;
                }
                else
                    Icons[i].GetComponent<SpriteRenderer>().sprite = null;

                if (player.Consumable != null)
                {
                    Sprite icon = player.Consumable.GetComponent<SpriteRenderer>().sprite;
                    Icons[2].GetComponent<SpriteRenderer>().sprite = icon;
                }
                else
                    Icons[2].GetComponent<SpriteRenderer>().sprite = null;
            }
			
			for (int i = 0; i < 2;i++)
			{
				if(player.Selected == player.Slots[i])
				{
				transform.position = Inventory[i].transform.position;
				}
			}
		}
	}

	void AnimateHighlight()
	{

		if (frame <= Anim_Highlight.Length-1)
		{
			time ++;
			if (time >= Anim_speed)
			{
				GetComponent<SpriteRenderer>().sprite = Anim_Highlight[frame++];
				time = 0;
			}
			if (frame == Anim_Highlight.Length-1)
			{
				if (frame != 0)
				{
					time ++;
					if (time >= Anim_speed)
					{
						GetComponent<SpriteRenderer>().sprite = Anim_Highlight[frame--];
						time = 0;
					}
				}
			}
		} else frame = 0;
			
	}

	/*if (frame <= Walk_Up_Left.Length-1)
	{
		time ++;
		if (time >= Anim_speed)
		{
			player.sprite = check[frame++];
			time = 0;
		}
	} else frame = 0;*/
}
