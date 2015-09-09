using UnityEngine;
using System.Collections;
using Database;

public class Player : MonoBehaviour {

	public enum states {Idle, Armed,Attacking, Searching, Hurt, Drawing}
	public enum player {one,two,three,four}
	public player P;
	public states State;
	public int Points, Health,WeaponHeld, Hits;
	public float Speed;
	float drawing, attacking, consuming, smoking, peeing;
	public bool IsBleeding, WeaponDrawn;
	public string Name;
	public Need[] Needs = new Need[4];
	public GameObject[] Slots = new GameObject[3];
	public GameObject Weapon;

	// Use this for initialization
	void Awake () 
	{
		Name = Get.Name;
		CreateNeeds();
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

	// Update is called once per frame
	void Update () 
	{
		if (P == player.one)
		{
		if (State == states.Idle || State == states.Armed)
		{
		if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.up * Speed * Time.deltaTime);
		if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right * Speed * Time.deltaTime);
		if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left * Speed * Time.deltaTime);
		if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.down * Speed * Time.deltaTime);
		}

		if (State == states.Idle)
		{
			if (Input.GetKeyDown(KeyCode.X) && Slots[0] != null) ReadyDraw(0);
			if (Input.GetKeyDown(KeyCode.C) && Slots[1] != null) ReadyDraw(1);
			if (Input.GetKeyDown(KeyCode.V) && Slots[2] != null) ReadyDraw(2);
		}

		if (State == states.Armed)
		{
			if (Input.GetKeyDown(KeyCode.Z)) Attack(WeaponHeld);
			if (Input.GetKeyDown(KeyCode.X) && Slots[0] != null) Undraw(0);
			if (Input.GetKeyDown(KeyCode.C) && Slots[1] != null) Undraw(1);
			if (Input.GetKeyDown(KeyCode.V) && Slots[2] != null) Undraw(2);
		} 

		if (State == states.Drawing)
		{
				if (drawing != 0) drawing--;
				else {
					
					State = states.Armed;
					Item drawnweapon = Weapon.GetComponent<Item>();
					drawnweapon.Drawn = true;
					drawnweapon.Notice.enabled = true;
				}
		}
		if (State == states.Attacking)
		{
			if (attacking != 0) attacking--;
			else {
					Item attackweapon = Weapon.GetComponent<Item>();
				attackweapon.Lethal = false;
					attackweapon.Range.enabled = false;
				State = states.Armed;
			}
		}
		}

	}

	void Attack(int selected)
	{
		Item attackweapon = Weapon.GetComponent<Item>();
		attackweapon.Lethal = true;
		attackweapon.Range.enabled = true;
		attacking = attackweapon.AttackSpeed;
		State = states.Attacking;
	}
	void ReadyDraw(int selected)
	{
		print("ARMING");
		State = states.Drawing;
		ClearDraws();
		Weapon = Instantiate(Slots[selected],transform.position,transform.rotation) as GameObject;
		Weapon.transform.parent = this.transform;
		Item drawnweapon = Weapon.GetComponent<Item>();
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
			Needs [i].Name = Get.NeedName [i];
		}
	}
	void Undraw(int selected)
	{
		Item heldweapon = Weapon.GetComponent<Item>();
		if (heldweapon.Drawn == true && heldweapon.Lethal == false)
		{
			heldweapon.Drawn = false;
			heldweapon.Notice.enabled = false;
			State = states.Idle;
		}
	}
}
