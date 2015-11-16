using UnityEngine;
using System.Collections;

public class Result : MonoBehaviour {

	static public bool End;
	static public string[] PlayerName = new string[4];
	static public int[] PlayerScore = new int[4];
    int press_count;
	public Rect ResultBox;
	public Rect[] PlayerDisplay = new Rect[4];
	public Rect NextRound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            End = true;
        }

        if (Network.connections.Length+1 == press_count)
        {
            Application.LoadLevel(1);
            Digit.currentRound++;
            Time.timeScale = 1;
            press_count = 0;
            for (int i = 0; i < Result.PlayerName.Length; i++)
            {
                Result.PlayerName[i] = null;
            }
        }
    }
	void OnGUI () 
	{
		if (End)
		{
            Time.timeScale = 0;
            GUI.Box(ResultBox, "");
            for (int i = 0; i < PlayerDisplay.Length; i++)
            {
                GUI.Label(PlayerDisplay[i], PlayerName[i] + "  " + PlayerScore[i]);
            }
            if (GUI.Button(NextRound, "Next Round"))
            {
                press_count++;
                End = false;
            }

        }
    }
}
