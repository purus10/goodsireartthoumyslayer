using UnityEngine;
using System.Collections;
using Database;

public class Player : MonoBehaviour {

	public int Points;
	public int Health;
	public bool IsBleeding;
	public string Name;
	public Need[] Needs = new Need[4];
	public Item[] Slots = new Item[3];

	// Use this for initialization
	void Awake () 
	{
		Name = Get.Name;
		CreateNeeds();

	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void CreateNeeds()
	{
		for (int i = 0; i< Needs.Length; i++) 
		{
			Needs[i] = new Need();
			Needs [i].Name = Get.NeedName [i];
		}
	}
}
