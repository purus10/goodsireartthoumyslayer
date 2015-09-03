using UnityEngine;
using System.Collections;
using Database;

public class Item : MonoBehaviour {

	public enum type {Consumable,Spawn,Weapon};
	public enum consumable{Poison, Snack, Drink, Bandage, Firecracker, PainKiller};

	public bool IsPoisoned;
	public string Name;
	public Collider Range;
	public type Type; 
	public consumable IsConsumable;
	public Item Loot;
	public int Ammo, Amount, AttackSpeed, Bleed, DrawSpeed, Force, Suspicion, ThrowAmount;

	//Check if Player is picking up or poisoning the loot
	void OnColliderEnter(Collider col )
	{
		if (Type == type.Spawn) 
		{
			Player player = col.GetComponent<Player> ();
			if (Input.GetKeyDown (KeyCode.S))
			{
				if (player.Slots[3] != null)
					if (player.Slots[3].IsConsumable == consumable.Poison && IsConsumable == consumable.Drink && IsPoisoned == false) 
				{
					IsPoisoned = true;
					player.Slots[3] = null;
				}
			} else if (player != null) 
			{
				if (Input.GetKeyDown (KeyCode.A)) GiveItem (player);
			}
		}
	}
	// Update is called once per frame
	void Update () 
	{

	}
	void GiveItem(Player player)
	{
		if (Loot.Type == type.Consumable) {
			if (player.Slots [3] == null)
			{
				if (IsPoisoned == true && Loot.IsConsumable == consumable.Drink) 
				{
					Loot.IsPoisoned = true;
					IsPoisoned = false;
				}
				player.Slots [3] = Loot;
			}
		} else if (Loot.Type == type.Weapon) 
		{
			if (player.Slots [1] == null) player.Slots [1] = Loot;
			else if (player.Slots [2] == null) player.Slots [2] = Loot;
		}
	}
	void CastItem(Player player)
	{
		if (Type == type.Consumable) 
		{
			if (IsConsumable == consumable.Drink) player.Needs [4].Meter += Amount;
			else if (IsConsumable == consumable.Snack) player.Needs [1].Meter += Amount;
			else if (IsConsumable == consumable.Bandage)
			{
				if (player.IsBleeding) player.IsBleeding = false;
			}
			else if (IsConsumable == consumable.Firecracker) Consumable.Firecracker();
			else if (IsConsumable == consumable.PainKiller) Consumable.PainKiller();
		}
	}
}
