using UnityEngine;
using System.Collections;
using Database;

public class HUD_Bar : MonoBehaviour {
	
	public enum types{Health, Eat, Smoke, Bathroom, Drunk}
	public types Type;
	public Sprite[] Bar;
	public Sprite[] Number;
	public GameObject NumSprite;
	Sprite ShowBar;
	Sprite ShowNumber;
	NetworkView NView;

	void Start()
	{
	}

	void Start()
	{
		NetworkView nView = GetComponent<NetworkView>();
		if(!nView.isMine) enabled = false;
	}

	void Update () 
	{
		if (Get.player != null)
		{
			if (Type == types.Health)
				ShowHealth();
			else if (Type == types.Eat)
				ShowNeed(0);
			else if (Type == types.Smoke)
				ShowNeed(1);
			else if (Type == types.Bathroom)
				ShowNeed(2);
			else if (Type == types.Drunk)
				ShowNeed(3);
			GetComponent<SpriteRenderer>().sprite = ShowBar;
			NumSprite.GetComponent<SpriteRenderer>().sprite = ShowNumber;
			
		}
	}
	void ShowHealth()
	{
		if (Get.player.Health == 10)
		{
			ShowBar = Bar[0];
			ShowNumber = Number[0];
		} else {
			ShowBar = Bar[Get.player.Health];
			ShowNumber = Number[Get.player.Health];
		}
	}
	
	void ShowNeed(int i)
	{
		//0 = eat, 1 = smoke, 2 = bathroom, 3 = drunkness
		ShowBar = Bar[Get.player.Needs[i].Meter/10];
		ShowNumber = Number[Get.player.Needs[i].Meter/10];
	}


}
