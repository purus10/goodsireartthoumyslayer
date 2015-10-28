using UnityEngine;
using System.Collections;
using Database;

public class Player : MonoBehaviour {

	public enum states {Idle, Armed, Attacking, Searching, Hurt, Drawing}
	public enum player {one,two,three,four}
	public states State;
	public int Points, Health, WeaponHeld, Hits;
	public float Speed, EatTimer, BathTimer, DrunkTimer, SmokeTimer;
	public Rect[] GUIHUD;
	float[] NeedTimers = new float[4];
	float drawing, attacking, consuming, smoking, peeing;
	public bool IsBleeding, WeaponDrawn, IsSeen, IsWanted, AxisPress;
	public string Name, TargetName;
	public Sprite TargetBody, TargetHead;
	public SpriteRenderer Body, Head;
	public Need[] Needs = new Need[4];
	public GameObject[] Slots = new GameObject[3];
	public CharacterController Character;
	public GameObject HUD;
	public GameObject Selected;
	public GameObject Weapon;
	NetworkView nView;
	int ResultSlot;

	// Use this for initialization
	void Awake ()
	{
		nView = GetComponent<NetworkView>();
		if(!nView.isMine) enabled = false;
		SetName ();
		CreateNeeds();
		SetDress ();
	}

	void OnCollisionEnter(Collision col)
	{
		Item item = col.gameObject.GetComponent<Item>();

		if (item != null && item.Lethal == true) 
		{
		Health -= item.Amount;
			print ("IM HIT!!!");
			item.Lethal = false;
		}
	}
	void Start()
	{
		if(Slots[0] != null && Selected == null) 
			Selected = Slots[0];
		GetComponentInChildren<Camera>().enabled = true;
		HUD.SetActive(true);
		StartCoroutine("PlayTimer");
		TargetName = Get.TargetName;
	}
	void FixedUpdate () 
	{
		Result.PlayerScore [ResultSlot] = Points;
		CheckNeeds();
		if (Input.GetButtonDown("A"))
			ToggleInventory();

		if (State == states.Idle || State == states.Armed)
		{
			Character.Move (Vector3.right * Input.GetAxis ("Horizontal") * Speed * Time.deltaTime);
			Character.Move (Vector3.up * Input.GetAxis ("Vertical") * Speed * Time.deltaTime);
		}

		if (State == states.Idle)
		{
			if (Input.GetButtonDown("Y"))
			{
					if (Selected == Slots[0]) ReadyDraw(0);
				else if (Selected == Slots[1]) ReadyDraw(1);
			}

			/*if (Input.GetKeyDown(KeyCode.X) && Slots[0] != null) Undraw(0);
			if (Input.GetKeyDown(KeyCode.C) && Slots[1] != null) Undraw(1);
			if (Input.GetKeyDown(KeyCode.V) && Slots[2] != null) Undraw(2);*/
		}

		if (State == states.Armed) 
		{
			/*if (Input.GetAxis ("RBumper") == 0)
				AxisPress = false;*/
			if (Input.GetAxis ("RBumper") != 0)
			{
				if (AxisPress == false)
				{
				Attack (WeaponHeld);
				AxisPress = true;
				}
			}

			if (Input.GetButtonDown("Y"))
			{
				if (Selected == Slots[0]) Undraw(0);
				else if (Selected == Slots[1]) Undraw(1);
			}
		}

		if (State == states.Drawing)
		{
				if (drawing != 0) drawing--;
				else {
				print("ARMING");
					State = states.Armed;
					Item drawnweapon = Weapon.GetComponentInChildren<Item>();
					drawnweapon.Drawn = true;
					drawnweapon.Notice.enabled = true;
				}
		}
		if (State == states.Attacking)
		{
			if (attacking != 0) 
				attacking--;
			else {
				AxisPress = false;
				State = states.Armed;
			}
		}

	}

	void SetName()
	{
		Name = Get.Name;
		for (int i = 0; i < Result.PlayerName.Length; i++) 
		{
			if (Result.PlayerName[i] == null)
			{
				Result.PlayerName[i] = Name;
				ResultSlot = i;
				break;
			}
		}
	}
	void SetDress()
	{
		GameObject clst = GameObject.Find ("Closet");
		Closet closet = clst.gameObject.GetComponent<Closet> ();
		closet.DressUp (this);
	}
	private IEnumerator PlayTimer()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);
		 	/*if (Network.isServer) 
				nView.RPC("TrackPlayTime",RPCMode.All);*/
		}
	}
	[RPC]
	private void TrackPlayTime()
	{
		//Digit.playTime++;
	}
	
	void ToggleInventory()
	{
		if (Slots[1] != null && Selected == Slots[0]) 
			Selected = Slots[1];
		else if (Selected == Slots[1] && Slots[0] != null)
			Selected = Slots[0];
	}
	void Attack(int selected)
	{
		Item attackweapon = Weapon.GetComponentInChildren<Item>();
			attackweapon.Lethal = true;
			attackweapon.Range.enabled = true;
			attacking = attackweapon.AttackSpeed;
			State = states.Attacking;
	}
	void ReadyDraw(int selected)
	{
		State = states.Drawing;
		ClearDraws();
		Weapon = Instantiate(Slots[selected],transform.position + Slots[selected].transform.position,Slots[selected].transform.rotation) as GameObject;
		Weapon.transform.parent = this.transform;
		Item drawnweapon = Weapon.GetComponentInChildren<Item>();
		drawing = drawnweapon.DrawSpeed;
		WeaponHeld = selected;
	}
	void ClearDraws()
	{
		GameObject.Destroy(Weapon);
		Weapon = null;
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
	void Undraw(int selected)
	{
		Item heldweapon = Weapon.GetComponentInChildren<Item>();
		if (heldweapon.Drawn == true && heldweapon.Lethal == false)
		{
			ClearDraws();
			State = states.Idle;
		}
	}
}
