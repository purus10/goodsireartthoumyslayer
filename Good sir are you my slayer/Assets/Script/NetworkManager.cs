using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {


	string registeredGameName = "Good_Sir_@rt_Thou_Sl@yer";
	float refreshRequestLength = 3.0f;
	bool spawn;
	public GameObject Player;
	HostData[] hostData;

	private void StartServer()
	{
		Network.InitializeServer(4,25000,false);
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

	void OnPlayerDisconnnect(NetworkPlayer player)
	{
		Debug.Log("Player disconnected from: "+ player.ipAddress + ":" + player.port);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
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
			Network.Disconnect(1);
			MasterServer.UnregisterHost();
		}
		
		if (Network.isClient) 
			Network.Disconnect(1);
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
		Network.Instantiate(Player, new Vector3(0f,0f,0f),Quaternion.identity,0);
	}

	public void OnGUI()
	{

		if (Network.isClient && spawn == false)
		{
			if (GUI.Button(new Rect(Screen.width/2,25f,150f,30f), "Spawn"))
				SpawnPlayer();
		}
		
		if (!Network.isClient && !Network.isServer)
		{
			if (GUI.Button(new Rect(Screen.width/2,25f,150f,30f), "Start New Server"))
			{
				StartServer();
			}
			
			if (GUI.Button(new Rect(Screen.width/2,85f,150f,30f), "Refresh Server List"))
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
