using UnityEngine;
using System.Collections;
using Database;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	public enum states {Idle, Armed, Attacking, Searching, Hurt, Drawing, Talking, Dead}
	public enum player {one,two,three,four}
    static public List<Vector3> SpawnPoints = new List<Vector3>();
    public List<Vector3> Spawn_Point;
    static public Player play;
    public states State;
    [SyncVar]
    public int Points;
    [SyncVar(hook = "OnDamage")]
    public int Health;
    public int WeaponHeld, Hits;
    public float Speed;
    float EatTimer, BathTimer, DrunkTimer, SmokeTimer;
    public Rect[] GUIHUD;
    public GameObject WeaponCollider;
    public BoxCollider[] WeaponRange;
    float[] NeedTimers = new float[4];
	public float drawing, attacking, consuming, smoking, peeing;
	public bool IsBleeding, WeaponDrawn, IsSeen, IsWanted, AxisPress;
    public SpriteRenderer[] Sprites;
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
    public Color self;
    public Color hurt;
    public int hitTimer,hitcount;
    public bool hit;
    public Camera PleaseStandby;
	int ResultSlot;
    public bool endgame;

    [Command]
    public void CmdStartLerp(int id)
    {
        print("I AM STARTING LOOP");
        Npc[] SearchN = GameObject.FindObjectsOfType(typeof(Npc)) as Npc[];
        for (int i = 0; i < SearchN.Length;i++)
        {
            if (SearchN[i].id == id)
            {
                SearchN[i].StartCoroutine(HurtLerp(id));
                break;
            }
        }
    }

    [Command]
    public void CmdDestroyUnit(int id)
    {
        Npc[] SearchN = GameObject.FindObjectsOfType(typeof(Npc)) as Npc[];

        for (int i = 0; i < SearchN.Length; i++)
        {
            if (SearchN[i].id == id)
            {
                NetworkServer.Destroy(SearchN[i].gameObject);
                break;
            }
        }
    }

    [ClientRpc]
    public void RpcDestroyUnit(int id)
    {
        Npc[] SearchN = GameObject.FindObjectsOfType(typeof(Npc)) as Npc[];

        for (int i = 0; i < SearchN.Length; i++)
        {
            if (SearchN[i].id == id)
            {
                NetworkServer.Destroy(SearchN[i].gameObject);
                break;
            }
        }
    }

    IEnumerator HurtLerp(int id)
    {
        int i = -1;
        Npc[] SearchN = GameObject.FindObjectsOfType(typeof(Npc)) as Npc[];

        for (int l = 0; l < SearchN.Length; l++)
        {
            if (SearchN[l].id == id)
            {
                l = i;
                break;
            }
            print(i);
        }

        if (i != -1 && SearchN[i].hurtstart == true)
        {
            print(i+" ID = "+id);
            for (int j = 0; j < SearchN[i].Sprite.Length; j++)
            {
                SearchN[i].Sprite[j].color = Color.Lerp(SearchN[i].Sprite[j].color, Color.red, 0.1f * Time.time);
            }
            yield return null;
        }
    }



    // Use this for initialization
    void Awake ()
	{
        self = Sprites[0].color;
        play = this;
        CreatePositionList();
		SetName();
		CreateNeeds();
		SetDress ();
	}

    void OnDamage(int newHealth)
    {
        if (newHealth < 10)
        {
            Health = newHealth;
        }
    }

    void TakeDamage(int damage)
    {
        if (Health - damage >= 0)
            Health -= damage;
        else Health = 0;
    }

	void OnTriggerStay(Collider col)
	{
        Player player = col.gameObject.GetComponentInParent<Player>();
        if (player != null && player != this)
        {
            Item item = player.gameObject.GetComponentInChildren<Item>();
            if (item != null && item.Lethal)
            {
                player.WeaponRange[item.facing].enabled = false;
                item.Lethal = false;
                hit = true;
                if (player != this)
                player.TakeDamage(item.Amount);
            }
        }


        /*transform.rotation = new Quaternion(0, 0, 0, 0);
        if (item != null && item.Lethal == true)
        {
            print("IM HIT "+name);
            Health--;
            item.Lethal = false;
            TakeDamage(item.Amount);
        }*/
    }
        void Start()
	{
        if (Slots[0] != null && Selected == null) 
			Selected = Slots[0];
        if (isLocalPlayer)
        {
            HUD.SetActive(true);
        }
    }

    void CmdResultScreen()
    {
        Result.End = true;
    }

    public override void OnStartServer()
    {
        PlacePlayer();
    }

    [ClientRpc]
    public void RpcEndGame()
    {
        Result.End = true;
    }
    [Command]
    public void CmdEndGame()
    {
        Result.End = true;
        endgame = true;
    }

    [Command]
    void CmdHitOn()
    {
        hit = true;
    }
    void Update () 
	{
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.position = new Vector3(transform.position.x,transform.position.y,-0.1f);

        if (Health <= 0)
            State = states.Dead;


        if (State == states.Dead)
        {
            PleaseStandby.gameObject.SetActive(true);
            for (int i = 0; i < Sprites.Length; i++)
            {
                Sprites[i].enabled = false;
            }
            GetComponentInChildren<Camera>().enabled = false;
            GetComponent<CharacterController>().enabled = false;
                Item item = GetComponentInChildren<Item>();
            if (item != null)
            item.gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
        }

        if (hit == true)
        {
            hitcount--;
            if (hitcount <= 0)
            {
                hitcount = hitTimer;
                hit = false;
            }
        }
        if (hit == true)
        {
            for (int i = 0; i < Sprites.Length;i++)
            {
                Sprites[i].color = hurt;
            }
        } else
        {
            for (int i = 0; i < Sprites.Length; i++)
            {
                Sprites[i].color = self;
            }
        }
        if (isServer)
        {
            if (endgame == true)
            {
                RpcEndGame();
                endgame = false;
            }
        }
            
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (isServer)
                RpcEndGame();
            else CmdEndGame();
        }

        if (!isLocalPlayer)
            return;

        if (GUI_Start.Start == false && Health > 0)
        {
            GetComponentInChildren<Camera>().enabled = true;
        }

        if (Health == 0)
        {
            print("I AM DEAD");
        }

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

            /*if (isLocalPlayer)
                CmdSetPos();
            else
            position = transform.position;*/
        }

        if (State == states.Talking)
        {
        }

		if (State == states.Idle)
		{
			if (Input.GetButtonDown("Y"))
			{
                if (Selected == Slots[0] && Slots[0] != null) ReadyDraw(0);
                else if (Selected == Slots[1] && Slots[1] != null) ReadyDraw(1);
			}

            if (Input.GetAxis("LBumper") != 0)
            {
                if (Selected == Slots[0] && Slots[0] != null) Drop(0);
                else if (Selected == Slots[1] && Slots[1] != null) Drop(1);
            }
        }

		if (State == states.Armed) 
		{
            if (Weapon != null)
            {
                Item wep = Weapon.GetComponent<Item>();
                wep.Attack_Anim = false;
            }
			if (Input.GetAxis ("RBumper") != 0)
			{
				if (AxisPress == false)
				{
				Attack (WeaponHeld);
                   State = states.Attacking;
                    AxisPress = true;
				}
			}
            if (Input.GetAxis("RBumper") == 0)
            {
                AxisPress = false;
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
				}
		}
		if (State == states.Attacking)
		{
			if (attacking != 0) 
				attacking--;
			else {
                if (Weapon != null)
                {
                    Item attackweapon = Weapon.GetComponentInChildren<Item>();
                    if (attackweapon.Lethal)
                    {
                        attackweapon.Attack_Anim = false;
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
        print(attackweapon);
			attackweapon.Lethal = true;
        attackweapon.Attack_Anim = true;
        WeaponRange[attackweapon.facing].enabled = true;
			attacking = attackweapon.AttackSpeed;
			State = states.Attacking;
	}


	void ReadyDraw(int selected)
	{
        State = states.Drawing;
        if (isServer)
        {
            SpawnWeapon(selected);
        } else CmdSpawnWeapon(selected);


    }
    [Command]
    void CmdSpawnWeapon(int selected)
    {
        CmdClearDraws();
        Weapon = (GameObject) GameObject.Instantiate(Slots[selected], transform.position, Quaternion.identity);
        NetworkServer.Spawn(Weapon);
        RpcParentWeapon(Weapon, selected);
    }

    [ClientRpc]
    void RpcParentWeapon(GameObject wep, int selected)
    {
        wep.transform.parent = this.transform;
        Item drawnweapon = wep.GetComponentInChildren<Item>();
        drawing = drawnweapon.DrawSpeed;
        WeaponHeld = selected;
        SetWeaponRange(drawnweapon.WeaponRange_Y, drawnweapon.WeaponRange_X);
        drawnweapon.Drawn = true;
        drawnweapon.Notice.enabled = true;
        Weapon = wep;
    }

    void SpawnWeapon(int selected)
    {
        CmdClearDraws();
        Weapon = GameObject.Instantiate(Slots[selected], transform.position, Quaternion.identity) as GameObject;
        Weapon.transform.parent = this.transform;
        Item drawnweapon = Weapon.GetComponentInChildren<Item>();
        drawing = drawnweapon.DrawSpeed;
        WeaponHeld = selected;
        SetWeaponRange(drawnweapon.WeaponRange_Y, drawnweapon.WeaponRange_X);
        drawnweapon.Drawn = true;
        drawnweapon.Notice.enabled = true;
        NetworkServer.Spawn(Weapon);
    }
    [Command]
    void CmdClearDraws()
	{
        print("I AMA DESTROYING");
		GameObject.Destroy(Weapon);
		Weapon = null;
	}

    [ClientRpc]
    void RpcClearDraws()
    {
        print("I AMA DESTROYING");
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
        print("I RUN");
		Item heldweapon = Weapon.GetComponent<Item>();
		if (heldweapon.Drawn == true && heldweapon.Lethal == false)
		{
            if (isLocalPlayer)
                CmdClearDraws();
            else
                RpcClearDraws();
			State = states.Idle;
		}
	}
}
