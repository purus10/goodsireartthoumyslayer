using UnityEngine;
using System.Collections;


namespace Database{

	public class Get
	{
		public enum Type {Head, Body, Accessory}; 
		static public string[] FirstName = new string[] {"Lol","JK","Hi","moo"};
		static public string[] LastName = new string[] {"L.","K.","J,","H."};
		static public string[] NeedName = new string[] {"Eat","Smoke","Bathroom","Drunkness"};
		static public string Name{get{return FirstName [Random.Range (0, FirstName.Length)]+" "+LastName [Random.Range (0, LastName.Length)];}}
	}

	public class Need
	{
		public string Name;
		public int Meter = 100;
	}

	public class Consumable
	{
		static public void Drink(Player player, int amount)
		{

		}
		static public void Snack()
		{
			
		}
		static public void Bandage()
		{
			
		}
		static public void Firecracker()
		{
		}
		static public void PainKiller()
		{
		}
	}

}
