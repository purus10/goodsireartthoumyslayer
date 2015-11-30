using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Unit_Spawner : NetworkBehaviour {

    [SerializeField] GameObject NPC_prefab;
    [SerializeField] GameObject GUARD_prefab;
    [SerializeField] GameObject BUTLER_prefab;
    [SerializeField] GameObject[] Item_prefab;
    [SerializeField] List<Vector3> SpawnPoints = new List<Vector3>();
    [SerializeField] List<Vector3> ItemSpawnPoints = new List<Vector3>();
    private int counter;
    public int NumberOfNPC;
    public int NumberOfGuards;
    public int NumberOfButlers;

    public override void OnStartServer()
    {
        for (int i = 0;i<NumberOfNPC; i++)
        {
            if (i < SpawnPoints.Count)
            {
                int position = Random.Range(0, SpawnPoints.Count - 1);
                SpawnNPC(SpawnPoints[position]);
                SpawnPoints.RemoveAt(position);
            }
        }

        for (int i = 0; i < NumberOfGuards; i++)
        {
            if (i < SpawnPoints.Count)
            {
                int position = Random.Range(0, SpawnPoints.Count - 1);
                SpawnGuard(SpawnPoints[position]);
                SpawnPoints.RemoveAt(position);
            }
        }

        for (int i = 0; i < NumberOfButlers; i++)
        {
            if (i < SpawnPoints.Count)
            {
                int position = Random.Range(0, SpawnPoints.Count - 1);
                SpawnGuard(SpawnPoints[position]);
                SpawnPoints.RemoveAt(position);
            }
        }
    }

    void SpawnNPC(Vector3 position)
    {
        GameObject go = GameObject.Instantiate(NPC_prefab, position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(go);
    }

    void SpawnGuard(Vector3 position)
    {
        GameObject go = GameObject.Instantiate(GUARD_prefab, position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(go);
    }

    void SpawnButler(Vector3 position)
    {
        GameObject go = GameObject.Instantiate(BUTLER_prefab, position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(go);
    }

    void SpawnItem(Vector3 position)
    {

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
