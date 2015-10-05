﻿using UnityEngine;
using System.Collections;
using Database;

public class Npc : MonoBehaviour {

	public enum states {Idle, Afraid, Talking, Eat, Smoke, Drink, Bathroom, Walk, SearchingForGuard};
	public states State;
	public int Health, Supsicion, EatTimer, BathTimer, DrunkTimer, SmokeTimer, Afraidat, Crave, ConvoLength;
	float[] NeedTimers = new float[4];
	public Transform Afraidof;
	public float Watch, Wait, Act, Speed;
	public string Name;
	public Vector3[] Move;
	Vector3 direction;
	public Player offender;
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
			GetComponent<NetworkView>().RPC("GetHurt",RPCMode.AllBuffered,item.Amount);
			item.Lethal = false;
		}
	}
	[RPC]
	private void GetHurt(int amount)
	{
		Health -= amount;
	}

	void OnTriggerStay(Collider col)
	{
		Item item = col.gameObject.GetComponent<Item>();
		Npc guest = col.gameObject.GetComponent<Npc>();
		Player player = col.gameObject.GetComponent<Player>();
		
		if (item != null)
		{
			offender = col.gameObject.GetComponentInParent<Player>();
			if (item.Lethal == true) 
			{
				Supsicion += 2;
				offender.IsSeen = true;
			} else if (item.Drawn == true) Supsicion ++;
		} else if (guest != null)
		{
			if (guest.State == states.Afraid)
				State = states.Afraid;
			else if (guest.State == states.Idle)
			{

			}
		}

		if (State == states.Idle)
		{
			if (player != null)
				if(player.State != Player.states.Armed)
			{
					if (Input.GetButtonDown("X"))
				{
					State = states.Talking;
					print("YOU ARE TALKING");
					offender = player;
					for (int i = 0; i < offender.Needs.Length;i++)
					{
						ConvoLength -= offender.Needs[i].Meter;
					}
					ConvoLength -= (offender.Health * 10);
				}
			}
		}

		if (Supsicion >= Afraidat && State != states.Afraid) 
		{
			offender.IsSeen = true;
			State = states.Afraid;
			Afraidof = offender.transform;
		}
	}

	void OnTriggerExit(Collider col)
	{
		Watch = 0;
		if (State != states.Afraid)
		{
			if (offender != null)
				offender.IsSeen = false;
			offender = null;
		}
		if (State == states.Talking)
			State = states.Idle;
	}

	// Update is called once per frame
	void Update () 
	{
		if (State == states.Afraid)
		{
			if (Afraidof != null)
			{
				float distance = Vector3.Distance(Afraidof.position,transform.position);
				if (distance < 20f)
				{
					transform.Translate(-Vector3.MoveTowards(transform.position,Afraidof.position, 5f) * Speed * Time.deltaTime);
				} else {
					State = states.Idle;
					Supsicion = 0;
					offender = Afraidof.gameObject.GetComponent<Player>();
					offender.IsSeen = false;
					offender = null;
					Afraidof = null;
				}
			}
		} else if (State == states.Idle && offender != null)
		{
			Watch++;
			if (Watch >= (Act*2))
			{
				State = states.SearchingForGuard;
				Watch = 0;
			}
		} else if (State == states.Idle);
		{
			Wait++;
			if (Wait >= Act)
			{
				for (int i = 0; i < Needs.Length;i++)
				{
					if (Needs[i].Meter <= Crave)
						GetNeedState(i);
				}
				if (State == states.Idle)
				{
				State = states.Walk;
				direction = Move[Random.Range(0,Move.Length-1)];
				}
				Wait = 0;
			}
		}

		if (State == states.Walk) 
		{
			Walk(direction);
			Wait++;
			if (Wait >= Act)
			{
				State = states.Idle;
				Wait = 0;
			}
		}


		if (State == states.Talking)
		{
			Wait++;
			if (Wait >= ConvoLength)
			{

			}
		}

		if (State == states.SearchingForGuard)
		{
			Guard[] Search = GameObject.FindObjectsOfType(typeof(Guard)) as Guard[];

			//continue here

		}
	}

	void Walk(Vector3 dir)
	{
		transform.Translate(dir * Speed * Time.deltaTime);
	}

	void CreateNeeds()
	{
		for (int i = 0; i< Needs.Length; i++) 
		{
			Needs[i] = new Need();
			Needs[i].Name = Get.NeedName [i];
			Needs [i].Name = Get.NeedName [i]; 
		}
		
		
	}
	void GetNeedState(int i)
	{
		if (i == 0)
			State = states.Eat;
		else if (i == 1)
			State = states.Smoke;
		else if (i == 2)
			State = states.Bathroom;
		else if (i == 3)
			State = states.Drink;
	}
	void SetNeedTimers()
	{
		//0 = eat, 1 = smoke, 2 = bathroom, 3 = drunkness
		if (NeedTimers [0] <= 0)
			NeedTimers [0] = EatTimer;
		if (NeedTimers [1] <= 0)
			NeedTimers [1] = SmokeTimer;
		if (NeedTimers [2] <= 0)
			NeedTimers [2] = BathTimer;
		if(NeedTimers [3] <= 0)
			NeedTimers [3] = DrunkTimer;
	}
	void CheckNeeds()
	{
		for (int i = 0; i < NeedTimers.Length; i++)
		{
			NeedTimers[i]--;
			if ( NeedTimers[i] <= 0)
			{
				if (Needs[i].Meter > 0)
					Needs [i].Meter--;
				SetNeedTimers();
			}
		}
	}
}
