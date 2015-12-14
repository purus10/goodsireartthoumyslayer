using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.Networking;
using Database;


public class Npc : NetworkBehaviour {

	public enum states {Idle, Afraid, Talking, Hungry, Smoke, Drink, Bathroom, Walk, SearchingForGuard, Reporting};
	public states State;
	public bool hurtstart;
	public Unit Unit;
    public float HurtTimer;
    float HungerTimer, BathTimer, DrunkTimer, SmokeTimer;

    [SyncVar(hook = "OnDamage")]
    public int Health;
    public int Suspicion, Afraidat, Crave;
	float[] needs = new float[4] {100,100,100,100};
	public float conversation = 500f, convoLength = 50f;
	float[] NeedTimers = new float[4];
	Guard[] Search;
	Item[] Items, selections;
	public LayerMask layermask;
	public GameObject SearchingforArea;
	public Transform Afraidof;
	public TextMesh  Namerender;
	public SpriteRenderer[] Sprite = new SpriteRenderer[2];
	public float SetCounter;
	public float counter = 50f, hurt = 0.5f;
	public float Watch, AfraidSpeed;
	public string Name;
	public Vector3[] Move;
	public CharacterController Character;
	public Vector3 direction;
	public Player offender;
    public Npc mingler;
    public SpriteBubble Bubble;
	public Need[] Needs = new Need[4];
    public int id;
    Player setplayer;
    int test;

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
        conversation = Random.Range(300, 700);
        convoLength = Random.Range(20, 40);
        HungerTimer = Random.Range(60, 120);
        BathTimer = Random.Range(60, 120);
        DrunkTimer = Random.Range(60, 120);
        SmokeTimer = Random.Range(60, 120);
        Name = Get.Name;
        id = Get.ID;
        Get.ID++;
 
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
        Player player = col.gameObject.GetComponentInParent<Player>();
        if (player != null)
        {
            Item item = player.gameObject.GetComponentInChildren<Item>();
            if (item != null && item.Lethal)
            {
                hurtstart = true;
                 player.WeaponRange[item.facing].enabled = false;
                 item.Lethal = false;
                player.CmdStartLerp(id);
                TakeDamage(item.Amount);
            }
        }
    }

    void OnDamage(int newHealth)
    {
        if (newHealth < 10)
        {
            Health = newHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        print(Health);
    }

    /*public IEnumerator HurtLerp()
	{
        if (hurtstart == true)
        {
            for (int i = 0; i < Sprite.Length; i++)
            {
                Sprite[i].color = Color.Lerp(Sprite[i].color, Color.red, 0.1f * Time.time);
            }

            yield return null;
        }
	}*/
	private void RevertColor()
	{
		for (int i = 0; i < Sprite.Length;i++)
		{
			Sprite[i].color = Color.white;
		}
	}

    void OnTriggerEnter(Collider col)
    {
        Npc guest = col.gameObject.GetComponent<Npc>();
        if (guest != null && mingler == null && SearchingforArea == null)
        {
            float distance = Vector3.Distance(guest.transform.position, transform.position);
            if (distance > 0.5f)
            {
                if (guest.State == states.Afraid)
                    State = states.Afraid;
                else if (guest.State == states.Idle)
                {
                    mingler = guest;
                    guest.mingler = this;
                    guest.State = states.Talking;
                    State = states.Talking;
                }
            } else
            {
                direction = Move[Random.Range(0, Move.Length)];
                Vector3 move = direction + new Vector3(direction.x * Random.Range(1f, 20f), direction.y * Random.Range(1f, 12f), -0.1f);
                bool walkable = (Physics.CheckSphere(move, 0.5f, layermask));
                if (walkable)
                {
                    State = states.Walk;
                    Unit.MoveTo(move);
                }
                else
                {
                    counter = SetCounter;
                }
            }
        }
    }

        void OnTriggerStay(Collider col)
	{
		Item item = col.gameObject.GetComponent<Item>();
		Player player = col.gameObject.GetComponent<Player>();
		
		if (item != null)
		{
			offender = col.gameObject.GetComponentInParent<Player>();
			if (item.Lethal == true) 
			{
				Suspicion += 2;
				offender.IsSeen = true;
			} else if (item.Drawn == true) Suspicion ++;
		}
		//TALKING TO PLAYER
		if (State == states.Idle)
		{
            if (player != null)
            {
                if (player.State != Player.states.Armed || player.State != Player.states.Attacking)
                {
                    if (Input.GetButtonDown("X") && State != states.Talking)
                    {
                        offender = player;
                        for (int i = 0; i < offender.Needs.Length; i++)
                            convoLength -= (offender.Needs[i].Meter / 10);
                        convoLength -= offender.Health;
                        State = states.Talking;
                        player.State = Player.states.Talking;
                    }
                }
            }
		}

		if (Suspicion >= Afraidat && State != states.Afraid) 
		{
            if (offender != null)
            {
                offender.IsSeen = true;
                Afraidof = offender.transform;
            }
			State = states.Afraid;		
		}
	}

	void OnTriggerExit(Collider col)
	{
        if (offender != null)
        {
            if (State != states.Afraid && offender.State != Player.states.Armed && offender.State != Player.states.Attacking)
            {

                offender.State = Player.states.Idle;
                offender.IsSeen = false;
                offender = null;
            }
        }
		if (State == states.Talking)
		{
			State = states.Idle;
		}
	}

   [ClientRpc]
   void RpcEndGame()
    {
        CmdEndGame();
    }
    [Command]
    void CmdEndGame()
    {
        Result.End = true;
        GameObject.Destroy(gameObject);
    }
    [Command]
    void CmdDestoryUnit()
    {
        NetworkServer.Destroy(this.gameObject);
    }
    [ClientRpc]
    void RpcDestoryUnit()
    {
        NetworkServer.Destroy(this.gameObject);
    }
    // Update is called once per frame
    void FixedUpdate () 
	{
        if (GUI_Start.Start)
            return;

		if (Health <= 0)
		{
			if (hurtstart == false)
            {
                for (int i = 0; i < Sprite.Length; i++)
                    Sprite[i].enabled = false;
                if (Name == Get.TargetName)
                {
                    if (isServer)
                        RpcEndGame();
                    else CmdEndGame();
                } else
                {
                    if (isServer)
                        Player.play.RpcDestroyUnit(id);
                    else
                        Player.play.CmdDestroyUnit(id);
                }
            }
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
		#region Hurt
		if (hurtstart == true)
		{
			hurt -= 0.1f;
			if (hurt <= 0)
			{
                RevertColor();
				hurt = 1f;
				hurtstart = false;
			}
		}
		#endregion
		#region Offended
		/*else if (State == states.Idle && offender != null)
		{
			Watch++;
			if (Watch >= 5f)
			{
				State = states.SearchingForGuard;
			}
		} */
		#endregion
		#region Idle
		else if (State == states.Idle);
		{
			if (counter <= 0)
			{
                if (mingler != null)
                {
                    float distance = Vector3.Distance(mingler.transform.position, transform.position);
                    if (distance > 1f)
                    {
                        Unit.MoveTo(mingler.transform.position);
                    } else
                    {
                        counter = SetCounter;
                        State = states.Talking;
                    }
                }

                    for (int i = 0; i < needs.Length; i++)
                    {
                        if (needs[i] <= Crave)
                            GetNeedState(i);
                    }
                    if (State == states.Idle)
                    {
                        direction = Move[Random.Range(0, Move.Length)];
                        Vector3 move = direction + new Vector3(direction.x * Random.Range(1f, 20f), direction.y * Random.Range(1f, 12f), -0.1f);
                        bool walkable = (Physics.CheckSphere(move, 0.5f, layermask));
                        if (walkable)
                        {
                            State = states.Walk;
                            Unit.MoveTo(move);
                        }
                        else
                        {
                            counter = SetCounter;
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
			if (Unit.path.Length > 0 && transform.position == Unit.path[Unit.path.Length-1])
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
            if (mingler != null)
            {

                if (counter <= 0)
                {
                    State = states.Idle;
                    mingler = null;
                }
            }
            if (offender != null)
            {
                InvokeRepeating("Talk", convoLength, 1.0f);
                if (conversation <= 0)
                {
                    Namerender.text = Name;
                }
            }
		}
		#endregion
		#region SearchingForGuard
	/*	if (State == states.SearchingForGuard)
		{
			if (Search == null)
				Search  = GameObject.FindObjectsOfType(typeof(Guard)) as Guard[];
			else {
                if (Search.Length > 0)
                {
                    bool walkable = (Physics.CheckSphere(Search[0].transform.position, 0.5f, layermask));
                    if (walkable)
                        Unit.MoveTo(Search[0].transform.position);
                    float distance = Vector3.Distance(Search[0].transform.position, transform.position);
                    if (distance < 1f)
                    {
                        Unit.MoveTo(transform.position);
                        State = states.Reporting;
                    }
                }
			}
		}*/
		#endregion
	/*	#region ReportingToGuard
		if (State == states.Reporting)
		{
			Search[0].Target = offender;
			State = states.Idle;
		}
		#endregion*/
		#region Drink
		if (State == states.Drink)
		{
            if (SearchingforArea == null)
            {
                SearchingforArea = GameObject.Find("Drink_Area");
                Unit.MoveTo(SearchingforArea.transform.position);
            }

            if (Unit.path.Length > 0 && transform.position == Unit.path[Unit.path.Length - 1])
            {
                needs[2] = 100;
                SearchingforArea = null;
                State = states.Idle;
            }
            /*if (Items == null)
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
			}*/
        }
		#endregion
		#region Bathroom
		if (State == states.Bathroom)
		{
			if (SearchingforArea == null)
			{
				SearchingforArea  = GameObject.Find("Toilet_Area");
				Unit.MoveTo(SearchingforArea.transform.position);
			}

			if (Unit.path.Length > 0 && transform.position == Unit.path[Unit.path.Length-1])
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
                SearchingforArea = GameObject.Find("Food_Area");
                Unit.MoveTo(SearchingforArea.transform.position);
            }

            if (Unit.path.Length > 0 && transform.position == Unit.path[Unit.path.Length - 1])
            {
                needs[2] = 100;
                SearchingforArea = null;
                State = states.Idle;
            }
            /*if (SearchingforArea == null)
			{
				SearchingforArea  = GameObject.Find("Food");
				Unit.MoveTo(SearchingforArea.transform.position);
			}

			if (transform.position == Unit.path[Unit.path.Length-1])
			{
				needs[0] = 100;
				SearchingforArea = null;
				State = states.Idle;
			}*/
        }
		#endregion
		#region Smoke
		if (State == states.Smoke)
		{
			if (SearchingforArea == null)
			{
				SearchingforArea  = GameObject.Find("Smoking_Area");
				Unit.MoveTo(SearchingforArea.transform.position);
			}

			if (Unit.path.Length > 0 && SearchingforArea != null && transform.position == Unit.path[Unit.path.Length-1])
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
