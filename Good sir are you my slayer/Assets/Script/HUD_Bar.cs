using UnityEngine;
using System.Collections;
using Database;

public class HUD_Bar : MonoBehaviour {
	
	public enum types{Health, Eat, Smoke, Bathroom, Drunk}
	public types Type;
	public Sprite[] Bar;
	public Sprite[] Number;
	public GameObject NumSprite;
	public Player player;
	Sprite ShowBar;
	Sprite ShowNumber;
	NetworkView NView;

	void Update () 
	{
		if (player != null)
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
		if (player.Health == 10)
		{
			ShowBar = Bar[0];
			ShowNumber = Number[0];
		} else {
			ShowBar = Bar[player.Health];
			ShowNumber = Number[player.Health];
		}
	}
	
	void ShowNeed(int i)
	{
        //0 = eat, 1 = smoke, 2 = bathroom, 3 = drunkness
        float meter = player.Needs[i].Meter;
        float meter_Roundup = Mathf.Round(meter / 10);
		ShowBar = Bar[(int)meter_Roundup];
		ShowNumber = Number[(int)meter_Roundup];
	}


}
