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
    [SerializeField] List<Vector3> SpawnPoints = new List<Vector3>();
    [SerializeField] List<Vector3> ItemSpawnPoints = new List<Vector3>();
    private int counter;
    public int NumberOfClues;
    public int NumberOfNPC;
    public int NumberOfGuards;
    public int NumberOfButlers;

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
                SpawnClue(ItemSpawnPoints[position]);
                ItemSpawnPoints.RemoveAt(position);
            }
        }
    }

    public override void OnStartServer()
    {
       //SpawnUnits();
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

    void SpawnClue(Vector3 position)
    {
        Instantiate(CLUE_prefab, position, Quaternion.identity);
    }

    // Use this for initialization
   /* void OnEnable () {
        SpawnUnits();
    }*/
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.A))
        {
            SpawnUnits();
        }
	
	}
}
