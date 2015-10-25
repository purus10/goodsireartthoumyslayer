using UnityEngine;
using System.Collections;
using Database;

public class Item : MonoBehaviour {

	public enum type {Consumable,Spawn,Weapon};
	public enum consumable{Any, Poison, Snack, Drink, Bandage, Firecracker, PainKiller};
	public bool IsPoisoned, Drawn, Lethal;
	public string Name;
	public Collider Range, Notice;
	public type Type; 
	public consumable IsConsumable;
	public GameObject Loot;
	public int Ammo, Amount, Bleed, Force, Suspicion, ThrowAmount;
	public float DrawSpeed, AttackSpeed;

	void Awake()
	{
		if (Type == type.Consumable)
		{
			if (IsConsumable != consumable.Any) Name = IsConsumable.ToString();
			else Name = Get.Consumable[Random.Range(0,Get.Consumable.Length)];
		}
	}

	//Check if Player is picking up or poisoning the loot
	void OnColliderEnter(Collider col)
	{
		if (Type == type.Spawn) 
		{
			Player player = col.GetComponent<Player> ();
			if (Input.GetKeyDown (KeyCode.C))
			{
				/*if (player.Slots[3] != null)
					if (player.Slots[3].IsConsumable == consumable.Poison && IsConsumable == consumable.Drink && IsPoisoned == false) 
				{
					IsPoisoned = true;
					player.Slots[3] = null;
				}*/
			} else if (player != null) 
			{
				if (Input.GetKeyDown (KeyCode.V)) 
					GiveLoot (player);
			}
		}
	}
	// Update is called once per frame
	void Update () 
	{
	}
	void GiveLoot(Player player)
	{
		Item loot = Loot.GetComponent<Item>();
		if (loot.Type == type.Consumable) 
		{
			if (player.Slots [3] == null)
			{
				if (IsPoisoned == true && loot.Name == "Drink") GivePoison();
				player.Slots[3] = Loot;
			}
		} else if (loot.Type == type.Weapon) GiveWeapon(player);
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
		if (player.Slots [1] == null) player.Slots [1] = Loot;
		else if (player.Slots [2] == null) player.Slots [2] = Loot;
	}
}
