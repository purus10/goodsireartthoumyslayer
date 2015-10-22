﻿using UnityEngine;
using System.Collections;
using Database;

public class Npc : MonoBehaviour {

	public enum states {Idle, Afraid, Talking, Eat, Smoke, Drink, Bathroom, Walk, SearchingForGuard, Reporting};
	public states State;
	public Unit Unit;
	public int Health, Suspicion, EatTimer, BathTimer, DrunkTimer, SmokeTimer, Afraidat, Crave, ConvoLength;
	float[] NeedTimers = new float[4];
	Guard[] Search;
	Item[] Items, selections;
	public GameObject SearchingforArea;
	public Transform Afraidof;
	public TextMesh  Namerender;
	public float Watch, Wait, Act, Speed, AfraidSpeed;
	public string Name;
	public Vector3[] Move;
	public CharacterController Character;
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
				Suspicion += 2;
				offender.IsSeen = true;
			} else if (item.Drawn == true) Suspicion ++;
		} else if (guest != null)
		{
			if (guest.State == states.Afraid)
				State = states.Afraid;
			else if (guest.State == states.Idle)
			{
				//Start talking to each other.
			}
		}

		if (State == states.Idle || State == states.Walk)
		{
			if (player != null)
				if(player.State != Player.states.Armed)
			{
					if (Input.GetButtonDown("X"))
				{
					State = states.Talking;
					Wait = 0;
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

		if (Suspicion >= Afraidat && State != states.Afraid) 
		{
			offender.IsSeen = true;
			State = states.Afraid;
			Afraidof = offender.transform;
		}
	}

	void OnTriggerExit(Collider col)
	{
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
		CheckNeeds ();
		#region Afraid
		if (State == states.Afraid)
		{
			if (Afraidof != null)
			{
				float distance = Vector3.Distance(Afraidof.position,transform.position);
				if (distance < 20f)
				{
					Character.Move (-Vector3.MoveTowards(transform.position,Afraidof.position, 5f) * AfraidSpeed * Time.deltaTime);
				} else {
					State = states.Idle;
					Suspicion = 0;
					//offender = Afraidof.gameObject.GetComponent<Player>();
					offender.IsSeen = false;
					offender = null;
					Afraidof = null;
				}
			}
		}
		#endregion
		#region Offended
		else if (State == states.Idle && offender != null)
		{
			Watch++;
			if (Watch >= (Act*2))
			{
				State = states.SearchingForGuard;
			}
		} 
		#endregion
		#region Idle
		else if (State == states.Idle);
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
				Wait = 0;
				}

			}
		}
		#endregion
		#region Walking
		if (State == states.Walk) 
		{
			if (Wait < Act)
			{
				Walk(direction*Random.Range(1f,10f));
				Wait++;
			} else {
				Wait = 0;
				State = states.Idle;
				if (Suspicion > 0) 
					Suspicion -= 5;
			}
		}
		#endregion
		#region Talking
		if (State == states.Talking)
		{
			if (Wait < ConvoLength)
				Wait++;
			else Namerender.text = Name;
		}
		#endregion
		#region SearchingForGuard
		if (State == states.SearchingForGuard)
		{
			print ("yes");
			if (Search == null)
				Search  = GameObject.FindObjectsOfType(typeof(Guard)) as Guard[];
			else {
				print ("yes");
				float distance = Vector3.Distance(Search[0].transform.position,transform.position);
				if (distance > 1f)
					Unit.MoveTo(Search[0].transform.position);
				else 
					State = states.Reporting;
			}
		}
		#endregion
		#region ReportingToGuard
		if (State == states.Reporting)
		{
			Search[0].Target = offender;

			State = states.Idle;
		}
		#endregion
		#region Drink
		if (State == states.Drink)
		{
			if (Items == null)
				Items  = GameObject.FindObjectsOfType(typeof(Item)) as Item[];
			else if (Needs[3].Meter <= 0)
			{
				Item drink = null;
				for (int i = 0; i < Items.Length;i++)
				{
					if (Items[i].Type == Item.type.Spawn && Items[i].Loot != null && Items[i].Loot.name == "Drink")
					{
						drink = Items[i];
						break;
					} else if (Items[i].IsConsumable == Item.consumable.Drink)
					{
						drink = Items[i];
						break;
					}
				}
				SearchingforArea = drink.gameObject;
				float distance = Vector2.Distance(drink.transform.position,transform.position);
				Unit.MoveTo(SearchingforArea.transform.position);
			} else
				State = states.Idle;
		}
		#endregion
		#region Bathroom
		if (State == states.Bathroom)
		{
			if (SearchingforArea != null && SearchingforArea.name == "Toilet")
			{
				Unit.MoveTo(SearchingforArea.transform.position);
			} else SearchingforArea  = GameObject.Find("Toilet");
		}
		#endregion
		#region Eat
		if (State == states.Eat)
		{
			if (SearchingforArea != null && SearchingforArea.name == "Food")
			{
				Unit.MoveTo(SearchingforArea.transform.position);
			} else SearchingforArea  = GameObject.Find("Food");
		}
		#endregion
		#region Smoke
		if (State == states.Smoke)
		{
			if (SearchingforArea != null && SearchingforArea.name == "Cigarette")
			{
				Unit.MoveTo(SearchingforArea.transform.position);
			} else SearchingforArea  = GameObject.Find("Cigarette");
		}
		#endregion
	}

	void Walk(Vector3 dir)
	{
		Unit.MoveTo (transform.position + dir);
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
		Wait = 0;
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
