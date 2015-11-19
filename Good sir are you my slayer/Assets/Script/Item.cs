using UnityEngine;
using System.Collections;
using Database;

public class Item : MonoBehaviour {

	public enum type {Consumable,Spawn,Weapon};
	public enum consumable{Any, Poison, Snack, Drink, Bandage, Firecracker, PainKiller};
	public bool IsPoisoned, Drawn, Lethal;
	public string Name;
    public Vector3[] Range_Position;
    public int facing;
	public Collider Notice, Range;
	public type Type; 
	public consumable IsConsumable;
	public GameObject Loot;
	public int Ammo, Amount, Bleed, Force, Suspicion, ThrowAmount;
	public float DrawSpeed, AttackSpeed;
    public Vector3 position;

    void Awake()
	{
		if (Type == type.Consumable)
		{
			if (IsConsumable != consumable.Any) Name = IsConsumable.ToString();
			else Name = Get.Consumable[Random.Range(0,Get.Consumable.Length)];
		}
	}

	//Check if Player is picking up or poisoning the loot
	void OnTriggerStay(Collider col)
	{
        Player player = col.GetComponent<Player>();

        if (Type == type.Spawn)
        { 
            if (player != null)
            {
                if (Input.GetKeyDown(KeyCode.J))
                    GiveLoot(player);
            }
		} else if (Type == type.Weapon && !Drawn)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                GiveSelf(player);
            }
        }
	}
	// Update is called once per frame
	void Update () 
	{
        if (position.x > transform.position.x)
        {
            //left
            facing = 6;
        }
        else if (position.x < transform.position.x)
        {
            //right
            facing = 2;
        }
        else if (position.y < transform.position.y)
        {
            //up
            facing = 0;
        }
        else if (position.y < transform.position.y && position.x < transform.position.x)
        {
            //upright
            facing = 1;
        }
        else if (position.y < transform.position.y && position.x > transform.position.x)
        {
            //upleft
            facing = 7;
        }
        else if (position.y > transform.position.y)
        {
            //down
            facing = 4;
        }
        else if (position.y > transform.position.y && position.x < transform.position.x)
        {
            //downright
            facing = 3;
        }
        else if (position.y > transform.position.y && position.x > transform.position.x)
        {
            //downleft
            facing = 5;
        }
        //Range.transform.position = Range_Position[facing];
        position = transform.position;
    }
    void GiveSelf(Player player)
    {
        //print("yes");
        if (player.Slots[0] == player.Selected && player.Slots[0] == null)
        {
            player.Slots[0] = Loot;
            //GameObject.Destroy(gameObject);
        }
        /*else if (player.Slots[1] == player.Selected && player.Slots[1] == null)
        {
            player.Slots[1] = Loot;
        }*/

    }
    void GiveLoot(Player player)
    {
        
        Item loot = Loot.GetComponent<Item>();
        if (loot != null)
        {
            if (loot.Type == type.Consumable)
            {
                if (player.Slots[2] == null)
                {
                    if (IsPoisoned == true && loot.Name == "Drink") GivePoison();
                    player.Slots[2] = Loot;
                }
            }
            else if (loot.Type == type.Weapon) GiveWeapon(player);
        }
    }
	void CastItem(Player player)
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
