using UnityEngine;
using System.Collections;


namespace Database{

	public class Get
	{
		public enum Type {Head, Body, Accessory}; 
		static public string[] FirstName = new string[] {"Lol","JK","Hi","moo"};
		static public string[] LastName = new string[] {"L.","K.","J,","H."};
		static public string[] NeedName = new string[] {"Eat","Smoke","Bathroom","Drunkness"};
		static public string[] Consumable = new string[] {"Poison","Snack","Drink","Bandage","Firecracker","PainKiller"};
		static public string Name{get{return FirstName [Random.Range (0, FirstName.Length)]+" "+LastName [Random.Range (0, LastName.Length)];}}
	}

	public class Need
	{
		public string Name;
		public int Meter = 100;
	}

	public class Cast
	{
		static public void Consumable(Item item, Player player)
		{
			if (item.Name == "Drink") Drink(item,player);
			else if (item.Name == "Snack") Snack(item,player);
			else if (item.Name == "Bandage") Bandage(player);
		}

		public class Spawn
		{

		}
		
		public class Weapon
		{
			
		}

		static public void Drink(Item item, Player player)
		{
			player.Needs [4].Meter += item.Amount;
		}

		static public void Snack(Item item, Player player)
		{
			player.Needs [1].Meter += item.Amount;
		}
		static public void Bandage(Player player)
		{
			if (player.IsBleeding) player.IsBleeding = false;
		}

	}
}
