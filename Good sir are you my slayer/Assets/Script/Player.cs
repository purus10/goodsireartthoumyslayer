using UnityEngine;
using Database;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	public enum states {Idle, Armed, Attacking, Searching, Hurt, Drawing, Talking}
	public enum player {one,two,three,four}
    static public List<Vector3> SpawnPoints = new List<Vector3>();
    public List<Vector3> Spawn_Point;
    public states State;
	public int Points, Health, WeaponHeld, Hits;
    public float Speed;
    float EatTimer, BathTimer, DrunkTimer, SmokeTimer;
    public Rect[] GUIHUD;
    public GameObject WeaponCollider;
    public BoxCollider[] WeaponRange;
    float[] NeedTimers = new float[4];
	public float drawing, attacking, consuming, smoking, peeing;
	public bool IsBleeding, WeaponDrawn, IsSeen, IsWanted, AxisPress;
	public string Name, TargetName;
	public SpriteRenderer TargetBody, TargetHead;
    public Player_Animations Anim;
	public Need[] Needs = new Need[4];
	public GameObject[] Slots = new GameObject[2];
    public GameObject Consumable;
	public CharacterController Character;
	public GameObject HUD;
	public GameObject Selected;
	public GameObject Weapon;
    public Container SelectedContain;
	int ResultSlot;

	// Use this for initialization
	void Awake ()
	{
        CreatePositionList();
		SetName();
		CreateNeeds();
		SetDress ();
	}

	void OnCollisionEnter(Collision col)
	{
		Item item = col.gameObject.GetComponent<Item>();

            if (item != null && item.Lethal == true)
            {
            Health--;
                item.Lethal = false;
            }
	}
        void Start()
	{
        if (Slots[0] != null && Selected == null) 
			Selected = Slots[0];
        GetComponentInChildren<Camera>().enabled = true;
        HUD.SetActive(true);
    }

    public override void OnStartServer()
    {
        PlacePlayer();
    }
    void Update () 
	{
        if (!isLocalPlayer)
            return;

        if (Health == 0)
        {
            GetComponent<Player>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
        }

       /* if (Input.GetKeyDown(KeyCode.A))
        {
            Application.LoadLevel("Main Scene");
            Player_Animations p = GetComponent<Player_Animations>();
            p.AssignParts();
            
            StartCoroutine("PlayTimer");
        }*/
        Result.PlayerScore [ResultSlot] = Points;
		CheckNeeds();
		if (Input.GetButtonDown("A"))
			ToggleInventory();

        if (Input.GetButtonDown("B"))
        {
            if (Consumable != null)
            {
                Item item = Consumable.GetComponent<Item>();
                item.CastItem(this);
            }
        }

        if (State == states.Idle || State == states.Armed || State == states.Talking)
		{
			Character.Move (Vector3.right * Input.GetAxis ("Horizontal") * Speed * Time.deltaTime);
			Character.Move (Vector3.up * Input.GetAxis ("Vertical") * Speed * Time.deltaTime);
		}

		if (State == states.Idle)
		{
			if (Input.GetButtonDown("Y"))
			{
					if (Selected == Slots[0] && Slots[0] != null) CmdReadyDraw(0);
				else if (Selected == Slots[1] && Slots[1] != null) CmdReadyDraw(1);
			}

            if (Input.GetAxis("LBumper") != 0)
            {
                if (Selected == Slots[0] && Slots[0] != null) Drop(0);
                else if (Selected == Slots[1] && Slots[1] != null) Drop(1);
            }
        }

		if (State == states.Armed) 
		{
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
                if (Weapon != null)
                {
                    Item attackweapon = Weapon.GetComponentInChildren<Item>();
                    attackweapon.Attack_Anim = false;
                    if (attackweapon.Lethal)
                    {
                        attackweapon.Lethal = false;
                        WeaponRange[attackweapon.facing].enabled = false;
                        attacking = attackweapon.AttackSpeed;
                    }
                    State = states.Armed;
                }
			}
		}

	}

    void CreatePositionList()
    {
        foreach (Vector3 p in Spawn_Point)
            SpawnPoints.Add(p);
    }

    public void PlacePlayer()
    {
        int spawnpoint = Random.Range(0, Spawn_Point.Count);
        transform.position = Spawn_Point[spawnpoint];
        Spawn_Point.RemoveAt(spawnpoint);

    }


    void SetWeaponRange(float Y, float X)
    {
        foreach (BoxCollider col in WeaponRange)
        {
            col.size = new Vector3(X, Y, 1f);
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
        Anim._head = Random.Range(0, Anim.Head_Walk_Down1.Length);
        Anim._body = Random.Range(0, Anim.Body_Walk_Down1.Length);
	}
	
	void ToggleInventory()
	{
		if (Selected == Slots[0]) 
			Selected = Slots[1];
		else if (Selected == Slots[1])
			Selected = Slots[0];
	}
	void Attack(int selected)
	{
		Item attackweapon = Weapon.GetComponentInChildren<Item>();
			attackweapon.Lethal = true;
        attackweapon.Attack_Anim = true;
        WeaponRange[attackweapon.facing].enabled = true;
			attacking = attackweapon.AttackSpeed;
			State = states.Attacking;
	}
    [Command]
	void CmdReadyDraw(int selected)
	{
		State = states.Drawing;
		ClearDraws();
        Weapon = GameObject.Instantiate(Slots[selected], transform.position, Quaternion.identity) as GameObject;
        Weapon.transform.parent = this.transform;
		Item drawnweapon = Weapon.GetComponentInChildren<Item>();
		drawing = drawnweapon.DrawSpeed;
		WeaponHeld = selected;
        SetWeaponRange(drawnweapon.WeaponRange_Y, drawnweapon.WeaponRange_X);
        NetworkServer.Spawn(Weapon);
    }
	void ClearDraws()
	{
		GameObject.Destroy(Weapon);
		Weapon = null;
	}

    void Drop(int selected)
    {
        if (Slots[selected] != null)
        {
            Item item = Slots[selected].GetComponent<Item>();
            GameObject go = GameObject.Instantiate(Slots[selected], transform.position, Quaternion.identity) as GameObject;
            NetworkServer.Spawn(go);
            Slots[selected] = null;
            Selected = null;
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
        EatTimer = Random.Range(20, 40);
        BathTimer = Random.Range(20, 40);
        DrunkTimer = Random.Range(20, 40);
        SmokeTimer = Random.Range(20, 40);
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
	public void Undraw(int selected)
	{
		Item heldweapon = Weapon.GetComponent<Item>();
		if (heldweapon.Drawn == true && heldweapon.Lethal == false)
		{
			ClearDraws();
			State = states.Idle;
		}
	}
}
