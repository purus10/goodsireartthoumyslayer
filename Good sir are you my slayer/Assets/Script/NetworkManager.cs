using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviour {
	
	string registeredGameName = "Good_Sir_@rt_Thou_Sl@yer";
	float refreshRequestLength = 3.0f;
    public LayerMask layermask;
    public int spawned;
    public bool spawn = false;
    public List<Vector3> Spawn_Point;
    public GameObject Player;
	HostData[] hostData;

	void Awake()
	{
        SpawnPlayer();
	}

	private void StartServer()
	{
		Network.InitializeServer(3,25000,false);
		MasterServer.RegisterHost(registeredGameName, "Let's Party");
	}

	void OnConnectedToServer()
	{
		Debug.Log("Connected to Server");
	}

	void OnServerInitialized()
	{
		Debug.Log("Server initialized");
		SpawnPlayer();
        spawn = true;
    }

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Debug.Log("Player disconnected from: "+ player.ipAddress + ":" + player.port);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
        spawned--;
	}
	void OnMasterServerEvent(MasterServerEvent masterServerEvent)
	{
		if (masterServerEvent == MasterServerEvent.RegistrationSucceeded)
			Debug.Log("Registration Successful");
	}

	void OnApplicationQuit()
	{
		if(Network.isServer)
		{
			Network.Disconnect(0);
			MasterServer.UnregisterHost();
		}
		
		if (Network.isClient) 
			Network.Disconnect(0);
	}

	public IEnumerator RefreshHotList()
	{
		Debug.Log("Tallying...");
		MasterServer.RequestHostList(registeredGameName);
		float timeEnd = Time.time + refreshRequestLength;

		while (Time.time < timeEnd)
		{
		hostData = MasterServer.PollHostList();
			yield return new WaitForEndOfFrame();
		}

		if (hostData == null || hostData.Length == 0) Debug.Log("No active servers.");
		else Debug.Log(hostData.Length + " have been found");
	}

    private void SpawnPlayer()
    {
        Debug.Log("Spawning Player...");
        int spawnpoint = Random.Range(0, Spawn_Point.Count);
        Network.Instantiate(Player, Spawn_Point[spawnpoint], Quaternion.identity, 0);
        Spawn_Point.RemoveAt(spawnpoint);

    }

	public void OnGUI()
	{

		if (Network.isClient && !spawn)
		{
			if (GUI.Button(new Rect(Screen.width/2,25f,150f,30f), "Spawn"))
			{
                SpawnPlayer();
                spawn = true;
                spawned++;
            }

		}

        if (Network.isServer && spawn)
        {
            if (GUI.Button(new Rect(Screen.width / 2, 25f, 150f, 30f), "Start Match"))
            {
                if (Network.connections.Length == spawned)
                {
                    Application.LoadLevel(1);
                }
            }
        }

        if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(Screen.width/2,25f,150f,30f), "Start New Server"))
			{
				StartServer();
			}
			
			if (GUI.Button(new Rect(Screen.width/2,50f,150f,30f), "Refresh Server List"))
			{
				StartCoroutine("RefreshHotList");
			}
			
			if (hostData != null)
			{
				for (int i = 0; i < hostData.Length;i++)
				{
					if (GUI.Button(new Rect(Screen.width/2,80f + (30f * i),130f,30f), hostData[i].gameName))
					{
						Network.Connect(hostData[i]);
					}
				}
			}
		}

	}
}
