using UnityEngine;
using System.Collections;
using Database;
using UnityEngine.Networking;

public class Item : NetworkBehaviour {

	public enum type {Consumable,Spawn,Weapon, Clue};
	public enum consumable{None, Any, Snack, Drink, PainKiller};
	public bool IsPoisoned, Drawn, Lethal, Attack_Anim, scary;
	public string Name;
    public int facing;
    public Vector3[] WeaponPlacment;
    public Vector3[] AttackPlacement;
	public Collider Notice;
	public type Type; 
	public consumable IsConsumable;
    public GameObject[] Consumables;
	public GameObject Loot;
    [SyncVar]
    public SpriteRenderer Weapon;
    public SpriteRenderer ItemSprite;
    public SpriteRenderer Attack;
    [SyncVar]
	public int Ammo, Amount, Bleed, Force, Suspicion, ThrowAmount;
	public float DrawSpeed, AttackSpeed, WeaponRange_X, WeaponRange_Y;
    public Vector3 position;

    void Awake()
	{
		if (Type == type.Consumable)
		{
			if (IsConsumable != consumable.None) Name = IsConsumable.ToString();
			else Name = Get.Consumable[Random.Range(0,Get.Consumable.Length)];
		}
	}


    void Start()
    {
        if (Type == type.Spawn && IsConsumable == consumable.Any)
        {
            int choice = Random.Range(0, Consumables.Length);

            switch (choice)
            {
                case 0:
                    IsConsumable = consumable.Drink;
                    Name = "Drink";
                    Loot = Consumables[choice];
                    break;
                case 1:
                    IsConsumable = consumable.Snack;
                    Name = "Snack";
                    Loot = Consumables[choice];
                    break;
                case 2:
                    IsConsumable = consumable.PainKiller;
                    Name = "PainKiller";
                    Loot = Consumables[choice];
                    break;

            }
            ItemSprite.sprite = Loot.GetComponent<SpriteRenderer>().sprite;
        }
    }

	//Check if Player is picking up or poisoning the loot
	void OnTriggerStay(Collider col)
	{
        Player player = col.GetComponent<Player>();

        if (player != null)
        {
            print("YEAH IM REGERSTING");
            if (Type == type.Spawn)
            {
                    if (Input.GetButtonDown("X"))
                        GiveLoot(player);
            }
            else if (Type == type.Weapon && player.Selected == null && player.State == Player.states.Idle || Type == type.Consumable)
            {
                if (Input.GetButtonDown("X"))
                {
                    GiveSelf(player);
                }
            }
            if (Type == type.Consumable)
            {
                if (Input.GetButtonDown("X"))
                    GiveLoot(player);
            }
        }
	}
    // Update is called once per frame
    void Update () 
	{
        if (Type == type.Weapon && Drawn == true)
        {
            if (Attack_Anim == true)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                Attack.enabled = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = true;
                Attack.enabled = false;
            }
            if (position.x > transform.position.x)
            {
                //left
                facing = 2;
                GetComponent<SpriteRenderer>().sortingOrder = 2;
                Attack.sortingOrder = 2;
                GetComponent<SpriteRenderer>().gameObject.transform.localPosition = WeaponPlacment[2];
                Attack.gameObject.transform.localPosition = AttackPlacement[2];
                Attack.gameObject.transform.localRotation = new Quaternion(0, 0, 180, 0);
            }
            else if (position.x < transform.position.x)
            {
                //right
                facing = 6;
                GetComponent<SpriteRenderer>().sortingOrder = 0;
                Attack.sortingOrder = -1;
                GetComponent<SpriteRenderer>().gameObject.transform.localPosition = WeaponPlacment[0];
                Attack.gameObject.transform.localPosition = AttackPlacement[2];
                Attack.gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            }
            else if (position.y < transform.position.y)
            {
                //up
                facing = 0;
                GetComponent<SpriteRenderer>().sortingOrder = 0;
                Attack.sortingOrder = -1;
                GetComponent<SpriteRenderer>().gameObject.transform.localPosition = WeaponPlacment[1];
                Attack.gameObject.transform.localPosition = AttackPlacement[3];
                Attack.gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            }
            else if (position.y < transform.position.y && position.x < transform.position.x)
            {
                //upright
                facing = 1;
                GetComponent<SpriteRenderer>().sortingOrder = 0;
                Attack.sortingOrder = -1;
                GetComponent<SpriteRenderer>().gameObject.transform.localPosition = WeaponPlacment[1];
                Attack.gameObject.transform.localPosition = AttackPlacement[2];
                Attack.gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            }
            else if (position.y < transform.position.y && position.x > transform.position.x)
            {
                //upleft
                facing = 7;
                GetComponent<SpriteRenderer>().sortingOrder = 0;
                Attack.sortingOrder = 2;
                GetComponent<SpriteRenderer>().gameObject.transform.localPosition = WeaponPlacment[2];
                Attack.gameObject.transform.localPosition = AttackPlacement[2];
                Attack.gameObject.transform.localRotation = new Quaternion(0, 0, 180, 0);
            }
            else if (position.y > transform.position.y)
            {
                //down
                facing = 4;
                GetComponent<SpriteRenderer>().sortingOrder = 2;
                Attack.sortingOrder = 2;
                GetComponent<SpriteRenderer>().gameObject.transform.localPosition = WeaponPlacment[0];
                Attack.gameObject.transform.localPosition = AttackPlacement[0];
                Attack.gameObject.transform.localRotation = new Quaternion(0, 0, 180, 0);
            }
            else if (position.y > transform.position.y && position.x < transform.position.x)
            {
                //downright
                facing = 3;
                GetComponent<SpriteRenderer>().sortingOrder = 2;
                Attack.sortingOrder = -1;
                GetComponent<SpriteRenderer>().gameObject.transform.localPosition = WeaponPlacment[0];
                Attack.gameObject.transform.localPosition = AttackPlacement[0];
                Attack.gameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            }
            else if (position.y > transform.position.y && position.x > transform.position.x)
            {
                //downleft
                facing = 5;
                GetComponent<SpriteRenderer>().sortingOrder = 2;
                Attack.sortingOrder = 2;
                GetComponent<SpriteRenderer>().gameObject.transform.localPosition = WeaponPlacment[0];
                Attack.gameObject.transform.localPosition = AttackPlacement[0];
            }
            position = transform.position;
        }
    }
    void GiveSelf(Player player)
    {
        if (Type == type.Weapon)
        {
            if (player.Slots[0] == null)
            {
                player.Slots[0] = Instantiate(Loot, new Vector3(0.3f, -0.1f, 1f), transform.rotation) as GameObject;
                RpcDestroy();
                player.Selected = player.Slots[0];
            }
            else if (player.Slots[1] == null)
            {
                player.Slots[1] = Instantiate(Loot, new Vector3(0.3f, -0.1f, 1f), transform.rotation) as GameObject;
                RpcDestroy();
                player.Selected = player.Slots[1];
            }
        } else if (player.Consumable == null)
        {
            player.Consumable = Instantiate(Loot, new Vector3(0.3f, -0.1f, 1f), transform.rotation) as GameObject;
            RpcDestroy();
        }
    }
    [ClientRpc]
    void RpcDestroy()
    {
        GameObject.Destroy(this.gameObject);
    }
    void GiveLoot(Player player)
    {
        
        Item loot = Loot.GetComponent<Item>();
        if (loot != null)
        {
            if (loot.Type == type.Consumable)
            {
                if (player.Consumable == null)
                {
                    player.Consumable = Loot;
                }
            }
            else if (loot.Type == type.Weapon) GiveWeapon(player);
        }
    }
	public void CastItem(Player player)
	{
		if (Type == type.Consumable) Cast.Consumable(this, player);
	}
	
	void GivePoison()
	{
		Item loot = Loot.GetComponent<Item>();
		loot.IsPoisoned = true;
		IsPoisoned = false;
	}

	void GiveWeapon(Player player)
	{
		if (player.Slots [0] == null) player.Slots[0] = Loot;
		else if (player.Slots [1] == null) player.Slots [1] = Loot;
	}
}
