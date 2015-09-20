using UnityEngine;
using System.Collections;

public class Name : MonoBehaviour {

	public Player player;
	public Sprite[] LetterSprite = new Sprite[26];
	string alphabit = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	public SpriteRenderer[] Letters; 
	char[] nameLetter;
	char[] alphaLetter;


	void Start () 
	{
		nameLetter = player.Name.ToCharArray();
		alphaLetter = alphabit.ToCharArray();
	}
	
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < nameLetter.Length;i++)
		{
			for (int j = 0; j < alphaLetter.Length;j++)
			{
				if (nameLetter[i] == alphaLetter[j]) 
					Letters[i].sprite = LetterSprite[j];
			}
		}
	}
}
