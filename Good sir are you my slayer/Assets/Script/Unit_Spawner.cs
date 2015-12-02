using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Unit_Spawner : NetworkBehaviour {

    [SerializeField] GameObject NPC_prefab;
    [SerializeField] GameObject GUARD_prefab;
    [SerializeField] GameObject BUTLER_prefab;
    [SerializeField] GameObject CLUE_prefab;
    [SerializeField] GameObject[] Item_prefab;
    [SerializeField] GameObject[] CONSUME_Prefab;
    public List<Vector3> SpawnPoints = new List<Vector3>();
    public List<Vector3> ItemSpawnPoints = new List<Vector3>();
    public GameObject startscreen;
    public List<Vector3> SaveSpawnPoints = new List<Vector3>();
    public List<Vector3> SaveItemSpawnPoints = new List<Vector3>();
    private int counter;
    public int NumberOfNPC;
    public int NumberOfGuards;
    public int NumberOfButlers;
    public int NumberOfItems;
    public int NumberOfClues;
    public int NumberOfConsumables;
    static public bool StartMatch;

    public void SpawnUnits()
    {
        for (int i = 0; i < NumberOfNPC; i++)
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

        for (int i = 0; i < NumberOfClues; i++)
        {
            if (i < ItemSpawnPoints.Count)
            {
                int position = Random.Range(0, ItemSpawnPoints.Count - 1);
                SpawnClues(ItemSpawnPoints[position]);
                ItemSpawnPoints.RemoveAt(position);
            }
        }

        for (int i = 0; i < NumberOfItems; i++)
        {
            if (i < ItemSpawnPoints.Count)
            {
                int position = Random.Range(0, ItemSpawnPoints.Count - 1);
                SpawnItem(ItemSpawnPoints[position]);
                ItemSpawnPoints.RemoveAt(position);
            }
        }

        for (int i = 0; i < NumberOfConsumables; i++)
        {
            if (i < ItemSpawnPoints.Count)
            {
                int position = Random.Range(0, ItemSpawnPoints.Count - 1);
                SpawnCpnsumables(ItemSpawnPoints[position]);
                ItemSpawnPoints.RemoveAt(position);
            }
        }
    }

    public override void OnStartServer()
    {
        
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
        GameObject go = GameObject.Instantiate(Item_prefab[Random.Range(0, Item_prefab.Length - 1)], position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(go);
    }

    void SpawnClues(Vector3 position)
    {
        Instantiate(CLUE_prefab, position, Quaternion.identity);
    }

    void SpawnCpnsumables(Vector3 position)
    {
        GameObject go = GameObject.Instantiate(CONSUME_Prefab[Random.Range(0, CONSUME_Prefab.Length - 1)], position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(go);
    }

    // Use this for initialization
    void Awake ()
    {
       /* foreach(Vector3 p in SpawnPoints)
        {
            SaveSpawnPoints.Add(p);
        }

        foreach(Vector3 p in ItemSpawnPoints)
        {
            SaveItemSpawnPoints.Add(p);
        }*/
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	    if (StartMatch == true)
        {
            SpawnUnits();
            StartMatch = false;
        }
	}
}
