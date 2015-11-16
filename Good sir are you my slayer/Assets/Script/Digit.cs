using UnityEngine;
using System.Collections;
using Database;

public class Digit : MonoBehaviour {
	
	int playTime;
	public Sprite[] Digits;
	public SpriteRenderer Minute;
	public SpriteRenderer Colon;
	public SpriteRenderer FirstSecond;
	public SpriteRenderer LastSecond;
	public SpriteRenderer CurrentRound;
	public SpriteRenderer FinalRound;
	public bool colon;
	public int lastSeconds = 0;
	public int firstSeconds = 0;
	public int minutes;
	static public int currentRound = 1;
	public int finalRound;
	SpriteRenderer digit;

	void Start()
	{
		StartCoroutine("ChangeDigits");
		digit = FinalRound;
		CreateDigit(finalRound, false);
	}


	// Use this for initialization
	void Update () 
	{
       if (minutes == 3)
            Result.End = true;

		digit = CurrentRound;
		CreateDigit(currentRound, false);
		digit = Minute;
		CreateDigit(minutes, false);
		digit = FirstSecond;
		CreateDigit(firstSeconds, false);
		digit = LastSecond;
		CreateDigit(lastSeconds, false);
		digit = Colon;
		CreateDigit(-1, colon);
	}

	private IEnumerator ChangeDigits()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);
			playTime++;
			lastSeconds = (playTime) % 10;
			firstSeconds = (playTime/10) % 6;
			minutes = (playTime/60) % 60;
			if (colon)
				colon = false;
			else colon = true;
			
		}
	}

	void CreateDigit(int i, bool b)
	{
		if (i == -1)
			digit.gameObject.SetActive(b);
		else if (i == 0)
			digit.sprite = Digits[0];
		else if (i == 1)
			digit.sprite = Digits[1];
		else if (i == 2)
			digit.sprite = Digits[2];
		else if (i == 3)
			digit.sprite = Digits[3];
		else if (i == 4)
			digit.sprite = Digits[4];
		else if (i == 5)
			digit.sprite = Digits[5];
		else if (i == 6)
			digit.sprite = Digits[6];
		else if (i == 7)
			digit.sprite = Digits[7];
		else if (i == 8)
			digit.sprite = Digits[8];
		else if (i == 9)
			digit.sprite = Digits[9];
	}

}