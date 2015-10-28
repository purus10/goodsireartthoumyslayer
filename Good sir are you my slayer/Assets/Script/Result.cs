using UnityEngine;
using System.Collections;

public class Result : MonoBehaviour {

	static public bool End;
	static public string[] PlayerName = new string[4];
	static public int[] PlayerScore = new int[4];
	public Rect ResultBox;
	public Rect[] PlayerDisplay = new Rect[4];
	public Rect NextRound;
	
	// Update is called once per frame
	void OnGUI () 
	{
		if (End)
		{
			//Time.timeScale = 0;
		GUI.Box(ResultBox,"");
			for (int i = 0; i < PlayerDisplay.Length;i++)
			{
				GUI.Label(PlayerDisplay[i], PlayerName[i]+ "  " + PlayerScore[i]);
			}
			if (GUI.Button(NextRound,"Next Round"))
			{
				Application.LoadLevel(0);
				Digit.currentRound++;
				End = false;
				for (int i = 0; i < Result.PlayerName.Length;i++)
				{
					Result.PlayerName[i] = null;
				}
			}

		}
	}
}
