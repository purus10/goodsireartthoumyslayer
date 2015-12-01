using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Result : NetworkBehaviour {

	static public bool End;
	static public string[] PlayerName = new string[4];
	static public int[] PlayerScore = new int[4];
    public Sprite Score_Board;
    public TextMesh[] Names = new TextMesh[4];
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
            if (Digit.currentRound < 3)
            {
                Application.LoadLevel(1);
                GetComponent<Unit_Spawner>().SpawnUnits();
                Digit.currentRound++;
                Digit.playTime = 0;
                Time.timeScale = 1;
                press_count = 0;


                for (int i = 0; i < Result.PlayerName.Length; i++)
                {
                    Result.PlayerName[i] = null;
                }
            } else
            {
                Network.Disconnect();
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

            if (Digit.currentRound != 3)
            {
                if (GUI.Button(NextRound, "Next Round"))
                {
                    press_count++;
                    End = false;
                }
            } else
            {
                if (GUI.Button(NextRound, "End Game"))
                {
                    press_count++;
                    End = false;
                }
            }

        }
    }
}
