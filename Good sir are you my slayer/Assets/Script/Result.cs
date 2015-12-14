using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Database;

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
    public Camera ResultScreen;
    string next = "Next Round";

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
                p.gameObject.SetActive(true);
                p.PlacePlayer();
                p.TargetBody.sprite = Clear;
                p.TargetHead.sprite = Clear;
                p.TargetName = "";
                p.Weapon = null;
                p.State = Player.states.Idle;
                p.AxisPress = false;
                p.Health = 10;
                p.PleaseStandby.gameObject.SetActive(false);
                for (int i = 0; i < p.Sprites.Length; i++)
                {
                    p.Sprites[i].enabled = true;
                }
                p.GetComponentInChildren<Camera>().enabled = true;
                p.GetComponent<CharacterController>().enabled = true;
                Item item = p.GetComponentInChildren<Item>();
                if (item != null)
                    GameObject.Destroy(item.gameObject);
                p.GetComponent<BoxCollider>().enabled = true;
                p.GetComponent<SphereCollider>().enabled = true;
                p.IsWanted = false;
                foreach (Need n in p.Needs)
                {
                    n.Meter = 100;
                }
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

    [ClientRpc]
    void RpcResultsNextRound()
    {
        Unit_Destroy();
        Digit.currentRound++;
        Digit.playTime = 0;
        Time.timeScale = 1;
        Unit_Spawner.StartMatch = true;
        End = false;
    }
    [ClientRpc]
    void RpcResultsEndGame()
    {
        Unit_Destroy();
        Digit.currentRound = 0;
        Time.timeScale = 1;
        Digit.playTime = 0;
        USpawner.startscreen.SetActive(true);
        USpawner.startscreen.GetComponent<Camera>().enabled = true;
        GUI_Start.Start = true;
        End = false;
    }

    void OnGUI ()
    {
        if (End)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 42;
            style.alignment = TextAnchor.MiddleCenter;
            GUI.color = Color.black;

            Player[] SearchP = GameObject.FindObjectsOfType(typeof(Player)) as Player[];
            foreach (Player p in SearchP)
            {
                p.GetComponentInChildren<Camera>().enabled = false;
                Time.timeScale = 0;

                for (int i = 0; i < PlayerDisplay.Length; i++)
                {
                    GUI.Label(PlayerDisplay[i], PlayerName[i] + "  " + PlayerScore[i], style);
                }

                if (Digit.currentRound != 3)
                {
                    if (isServer)
                    {
                        if (GUI.Button(NextRound, "Next Round"))
                        {
                            RpcResultsNextRound();
                            End = false;

                        }
                    }
                }
                else
                {
                    if (isServer)
                    {
                        if (GUI.Button(NextRound, "End Game"))
                        {

                            RpcResultsEndGame();
                            End = false;
                        }
                    }
                }
            }
        }

    }
}
