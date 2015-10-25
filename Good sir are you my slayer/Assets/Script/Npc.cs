﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Database;

public class Npc : MonoBehaviour {

	public enum states {Idle, Afraid, Talking, Hungry, Smoke, Drink, Bathroom, Walk, SearchingForGuard, Reporting};
	public states State;
	public Unit Unit;
	public float HungerTimer, BathTimer, DrunkTimer, SmokeTimer;
	public int Health, Suspicion, Afraidat, Crave;
	float[] needs = new float[4] {100,100,100,100};
	public float conversation = 100, convoLength = 50f;
	float[] NeedTimers = new float[4];
	Guard[] Search;
	Item[] Items, selections;
	public LayerMask layermask;
	public GameObject SearchingforArea;
	public Transform Afraidof;
	public TextMesh  Namerender;
	public float SetCounter;
	public float counter;
	public float Watch, AfraidSpeed;
	public string Name;
	public Vector3[] Move;
	public CharacterController Character;
	public Vector3 direction;
	public Player offender;
	public Need[] Needs = new Need[4];

	Vector3 RunAway()
	{
		Vector3 path = Vector3.zero;
		Vector3 moveaway = -Vector3.MoveTowards(transform.position,Afraidof.position,10f);
		bool walkable = (Physics.CheckSphere(moveaway,0.5f,layermask));
		if (walkable)
			path = moveaway;
		return path;
	}
	
	void Awake () 
	{

		InvokeRepeating("CountDown",1.0f,1.0f);
		InvokeRepeating("Hunger",HungerTimer,1.0f);
		InvokeRepeating("Smoke",SmokeTimer,1.0f);
		InvokeRepeating("Bathroom",BathTimer,1.0f);
		InvokeRepeating("Drunk",DrunkTimer,1.0f);
	}

	void CountDown()
	{
		counter--;
	}

	void Hunger()
	{
		needs[0]--;
	}

	void Smoke()
	{
		needs[1]--;
	}

	void Bathroom()
	{
		needs[2]--;
	}

	void Drunk()
	{
		needs[3]--;
	}

	void Talk()
	{
		conversation--;
	}

	void OnCollisionEnter (Collision col)
	{
		Item item = col.gameObject.GetComponent<Item>();
		print ("yes");
		print (item);

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
		//TALKING TO PLAYER
		if (State == states.Idle)
		{
			if (player != null)
				if(player.State != Player.states.Armed)
			{
					if (Input.GetButtonDown("X") && State != states.Talking)
				{
					offender = player;
					for (int i = 0; i < offender.Needs.Length;i++)
						convoLength -= (offender.Needs[i].Meter/10);
					convoLength -= offender.Health;
					State = states.Talking;
					print (convoLength);
					Namerender.text = "...Talking";
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
		{
			State = states.Idle;
			if (Namerender.text == "...Talking")
			{
				conversation = 100f;
				convoLength = 500f;
				Namerender.text = "";
			}

		}
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if (Health <= 0)
		{
			if (Name == Get.TargetName)
			{
				Application.LoadLevel(0);
				Digit.currentRound++;
			}
			GameObject.Destroy(gameObject);
		}

		#region Afraid
		if (State == states.Afraid)
		{
			if (Afraidof != null)
			{
				float distance = Vector3.Distance(transform.position,Afraidof.position);
				if (distance < 10f)
				{
						Unit.MoveTo(RunAway());
				} else {
					State = states.Idle;
					Suspicion = 0;
					offender = Afraidof.gameObject.GetComponent<Player>();
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
			/*Watch++;
			if (Watch >= (Act*2))
			{
				State = states.SearchingForGuard;
			}*/
		} 
		#endregion
		#region Idle
		else if (State == states.Idle);
		{
			if (counter == 0)
			{
				for (int i = 0; i < needs.Length;i++)
				{
					if (needs[i] <= Crave)
						GetNeedState(i);
				}
				if (State == states.Idle)
				{
					direction = Move[Random.Range(0,Move.Length-1)];
					Vector3 move = direction + new Vector3(1*Random.Range(1f,20f),1*Random.Range(1f,12f),-0.1f);
					bool walkable = (Physics.CheckSphere(move,0.5f,layermask));
					if (walkable)
					{
						Unit.MoveTo(move);
						State = states.Walk;
					}
				}

			}
		}
		#endregion
		#region Walking
		if (State == states.Walk) 
		{
			if (Unit.path.Length > 0)
			{
			if (transform.position == Unit.path[Unit.path.Length-1])
				{
				counter = SetCounter;
				State = states.Idle;
				if (Suspicion > 0) 
					Suspicion -= 5;
				}
			} else {
				counter = SetCounter;
				State = states.Idle;
			}
		}
		#endregion
		#region Talking
		if (State == states.Talking)
		{
			InvokeRepeating("Talk",convoLength,1.0f);
			if (conversation == 0)
			{
				Namerender.text = Name;
			}
		}
		#endregion
		#region SearchingForGuard
		if (State == states.SearchingForGuard)
		{
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
			} 

			if (transform.position == Unit.path[Unit.path.Length-1])
			{
				needs[3] = 100;
				SearchingforArea = null;
				State = states.Idle;
			}
		}
		#endregion
		#region Bathroom
		if (State == states.Bathroom)
		{
			if (SearchingforArea == null)
			{
				SearchingforArea  = GameObject.Find("Toilet");
				Unit.MoveTo(SearchingforArea.transform.position);
			}

			if (transform.position == Unit.path[Unit.path.Length-1])
			{
				needs[2] = 100;
				SearchingforArea = null;
				State = states.Idle;
			}
		}
		#endregion
		#region Hungry
		if (State == states.Hungry)
		{
			if (SearchingforArea == null)
			{
				SearchingforArea  = GameObject.Find("Food");
				Unit.MoveTo(SearchingforArea.transform.position);
			}

			if (transform.position == Unit.path[Unit.path.Length-1])
			{
				needs[0] = 100;
				SearchingforArea = null;
				State = states.Idle;
			}
		}
		#endregion
		#region Smoke
		if (State == states.Smoke)
		{
			if (SearchingforArea == null)
			{
				SearchingforArea  = GameObject.Find("cigarette");
				Unit.MoveTo(SearchingforArea.transform.position);
			}

			if (transform.position == Unit.path[Unit.path.Length-1])
			{
				needs[1] = 100;
				SearchingforArea = null;
				State = states.Idle;
			}
		}
		#endregion
	}

	void Walk(Vector3 dir)
	{
		Unit.MoveTo (transform.position + dir);
	}
	void GetNeedState(int i)
	{
		if (i == 0)
			State = states.Hungry;
		else if (i == 1)
			State = states.Smoke;
		else if (i == 2)
			State = states.Bathroom;
		else if (i == 3)
			State = states.Drink;
	}
}
