using UnityEngine;
using System.Collections;
using Database;

public class Npc : MonoBehaviour {

	public int Health, Supsicion;
	public string Name;
	public Need[] Needs = new Need[4];
	
	void Awake () 
	{
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
