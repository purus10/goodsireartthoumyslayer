using UnityEngine;
using System.Collections;
using Database;

public class Npc : MonoBehaviour {

	public enum states {Idle, Afraid, StartTalk, Talking, Curious, SearchingForGuard};
	public states State;
	public int Health, Supsicion, EatTimer, BathTimer, DrunkTimer, SmokeTimer, Afraidat;
	float[] NeedTimers = new float[4];
	public Transform Afraidof;
	public float Watch, Wait, Act, Speed;
	public string Name;
	public Vector3[] Move;
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
	}

	// Update is called once per frame
	void Update () 
	{
		if (offender != null && State == states.Idle)
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
				transform.Translate(Move[Random.Range(0,Move.Length)] * Speed * Time.deltaTime);
				Wait = 0;
			}
		}

		if (State == states.Afraid);
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
		}

		if (State == states.SearchingForGuard)
		{
			Guard[] Search = GameObject.FindObjectOfType(typeof(Guard)) as Guard[];

			//continue here

		}
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
