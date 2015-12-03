using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Result : NetworkBehaviour {

	static public bool End;
	static public string[] PlayerName = new string[4];
	static public int[] PlayerScore = new int[4];
    public Sprite Score_Board;
    public Sprite Clear;
    public TextMesh[] Names = new TextMesh[4];
    static public int press_count;
	public Rect ResultBox;
	public Rect[] PlayerDisplay = new Rect[4];
	public Rect NextRound;
    public Unit_Spawner USpawner;

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
                Unit_Destroy();
                Digit.currentRound++;
                Digit.playTime = 0;
                Time.timeScale = 1;
                Unit_Spawner.StartMatch = true;
                press_count = 0;

                for (int i = 0; i < Result.PlayerName.Length; i++)
                {
                    Result.PlayerName[i] = null;
                }
            } else
            {
                Unit_Destroy();
                Digit.currentRound = 0;
                print("GAME DONE");
                Time.timeScale = 1;
                Digit.playTime = 0;
                USpawner.startscreen.SetActive(true);
                USpawner.startscreen.GetComponent<Camera>().enabled = true;
                GUI_Start.Start = true;
            }
        }
    }

    void Unit_Destroy()
    {
        
        Guard[] SearchG = GameObject.FindObjectsOfType(typeof(Guard)) as Guard[];
        Butler[] SearchB = GameObject.FindObjectsOfType(typeof(Butler)) as Butler[];
        Npc[] SearchN = GameObject.FindObjectsOfType(typeof(Npc)) as Npc[];
        Item[] SearchI = GameObject.FindObjectsOfType(typeof(Item)) as Item[];
        Clue[] SearchC = GameObject.FindObjectsOfType(typeof(Clue)) as Clue[];
        Player[] SearchP = GameObject.FindObjectsOfType(typeof(Player)) as Player[];

        foreach (Guard g in SearchG)
        {
            GameObject.Destroy(g.gameObject);
        }
        foreach (Butler g in SearchB)
        {
            GameObject.Destroy(g.gameObject);
        }
        foreach (Npc g in SearchN)
        {
            GameObject.Destroy(g.gameObject);
        }
        foreach (Item g in SearchI)
        {
            GameObject.Destroy(g.gameObject);
        }
        foreach (Clue g in SearchC)
        {
            GameObject.Destroy(g.gameObject);
        }

        foreach(Vector3 p in USpawner.SaveSpawnPoints)
        {
            USpawner.SpawnPoints.Add(p);
        }
        foreach (Vector3 p in USpawner.SaveItemSpawnPoints)
        {
            USpawner.ItemSpawnPoints.Add(p);
        }

        if (Digit.currentRound < 3)
        {
            foreach (Player p in SearchP)
            {
                p.PlacePlayer();
                p.TargetBody.sprite = Clear;
                p.TargetHead.sprite = Clear;
                p.TargetName = "";
                p.Weapon = null;
                p.State = Player.states.Idle;
                p.AxisPress = false;
            }

            USpawner.SpawnPoints.Clear();
            foreach (Vector3 pos in USpawner.SaveSpawnPoints)
            {
                USpawner.SpawnPoints.Add(pos);
            }

            USpawner.ItemSpawnPoints.Clear();
            foreach (Vector3 pos in USpawner.SaveItemSpawnPoints)
            {
                USpawner.ItemSpawnPoints.Add(pos);
            }

        } else
        {
            foreach (Player p in SearchP)
            {
                foreach (Vector3 pos in Player.SpawnPoints)
                {
                    p.Spawn_Point.Add(pos);
                }
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
