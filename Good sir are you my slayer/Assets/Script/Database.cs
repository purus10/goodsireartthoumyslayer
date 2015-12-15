using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace Database{

	public class Get
	{
		public enum Type {Head, Body, Accessory}; 
		static public string TargetName;
		static public Sprite TargetHead;
		static public Sprite TargetBody;
		static public string[] FirstName = new string[] {"Collin","Conway","Ruby","Ripley","Janine","Otis","Crystal","Lewis","Ramsey","Juliana","Erin","Deborah","Jordon","Natalie","Bertina","Faron","Kerena","Cal","Fallon","Felicia","Madonna","Laurene","Rozanne","Kelia",
"Joselyn","Lindon","Layne","Ainslee","Abbi","Autumn","Elenora","Chris","Rudolph","Katelynn","Eveline","Wiley","Chase","Kristin","Noelene","Jepson","Ridley","Keegan","Blair","Harris","Michael",
"Joann","Grayson","Callahan","Leigh","Candice",
"JoBeth",
"Martie",
"Vlad",
"Farida",
"Lilia",
"Zoya",
"Bibiana",
"Fouad",
"Rossella",
"Karima",
"Sabah",
"Konstantin",
"Alisa",
"Raisa",
"Isotta",
"Hasim",
"Kesha",
"Rossana",
"Amir",
"Marcello",
"Adel",
"Qadir",
"Ilia",
"Lamya",
"Galya",
"Nasim",
"Kamal",
"Albina",
"Kistna",
"Vina",
"Shanti",
"Yami",
"Prem",
"Meena",
"Vikram",
"Priya",
"Om","Rajni"};
		static public string[] LastName = new string[] {"L.","K.","J.,","H."};
		static public string[] NeedName = new string[] {"Eat","Smoke","Bathroom","Drunkness"};
		static public string[] Consumable = new string[] {"Poison","Snack","Drink","Bandage","Firecracker","PainKiller"};
		static public string Name{get{return FirstName [Random.Range (0, FirstName.Length)]+" "+LastName [Random.Range (0, LastName.Length)];}}
        static public int ID;

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
			else if (item.Name == "PainKiller") Bandage(player);
		}

		public class Spawn
		{

		}
		
		public class Weapon
		{
			
		}

		static public void Drink(Item item, Player player)
		{
            player.Needs[3].Meter = Mathf.Min(player.Needs[3].Meter + item.Amount, 100);
            player.Consumable = null;
		}

		static public void Snack(Item item, Player player)
		{
            if (player.Health + 1 <= 10)
                player.Health = player.Health + 1;
            else player.Health = 10;
            player.Needs[0].Meter = Mathf.Min(player.Needs[0].Meter + item.Amount, 100);
            player.Consumable = null;
        }
		static public void Bandage(Player player)
		{
            if (player.Health + 3 <= 10)
                player.Health = player.Health + 3;
            else player.Health = 10;
            player.Consumable = null;
        }

	}
}
