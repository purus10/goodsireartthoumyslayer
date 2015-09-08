﻿using UnityEngine;
using System.Collections;
using Database;

public class Npc : MonoBehaviour {

	public enum states {Idle, Afraid, Talking, Curious};
	public states State;
	public int Health, Supsicion;
	public Transform Afraidof;
	public float Wait, Act, Speed;
	public string Name;
	public Vector3[] Move;
	public Need[] Needs = new Need[4];
	
	void Awake () 
	{
		CreateNeeds();
	}

	void OnCollisionEnter(Collision col)
	{
		Item item = col.gameObject.GetComponent<Item>();
		
		if (item != null && item.Lethal == true) 
		{
			Health -= item.Amount;
		}
	}

	void OnTriggerStay(Collider col)
	{
		Item item = col.gameObject.GetComponent<Item>();
		
		if (item != null)
		{
			if (item.Lethal == true) Supsicion += 2;
			if (item.Drawn == true) Supsicion ++;
		}

		if ( Supsicion >= 5000) 
		{
			State = states.Afraid;
			Transform player = col.gameObject.GetComponentInParent<Transform>();
			Afraidof = player;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (State == states.Idle);
		{
			Wait++;
			if (Wait >= Act)
			{
				transform.Translate(Move[Random.Range(0,Move.Length)] * Speed * Time.deltaTime);
				Wait = 0;
			}
		}


		if (State == states.Afraid);
		{
			if (Afraidof != null)
			{
				//Be AFraid :P
			}
		}
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
