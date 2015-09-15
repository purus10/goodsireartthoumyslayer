using UnityEngine;
using System.Collections;

public class HUD_Bar : MonoBehaviour {

	public enum types{Health, Eat, Smoke, Bathroom, Drunk}
	public types Type;
	static public Player player;
	public Sprite[] Bar;
	public Sprite[] Number;
	public GameObject NumSprite;
	Sprite ShowBar;
	Sprite ShowNumber;

	// Use this for initialization
	void Start () 
	{
		
	}

	// Update is called once per frame
	void Update () 
	{
		if (player != null)
		{
			if (Type == types.Health)
				ShowHealth();
		//	else if (Type == types.Eat
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
		} else if (player.Needs.{
			ShowBar = Bar[player.Health];
			ShowNumber = Number[player.Health];
		}
	}
}
