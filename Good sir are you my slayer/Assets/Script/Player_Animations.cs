using UnityEngine;
using System.Collections;
using Database;

public class Player_Animations : MonoBehaviour {
	
	SpriteRenderer player;
	public float Anim_speed;
	float time;
	int frame;
	public Sprite[] Walk_Up;
	public Sprite[] Walk_Up_Left;
	public Sprite[] Walk_Up_Right;
	public Sprite[] Walk_Down;
	public Sprite[] Walk_Down_Left;
	public Sprite[] Walk_Down_Right;
	public Sprite[] Walk_Left;
	public Sprite[] Walk_Right;

	void Start()
	{
		player = Get.player.GetComponent<SpriteRenderer>();
	}
	void Update () 
	{
	
		//walk
		if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
			Walk(Walk_Up_Left);
		else if(Input.GetKey(KeyCode.W)&& Input.GetKey(KeyCode.D))
			Walk(Walk_Up_Right);
		else if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
			Walk(Walk_Down_Right);
		else if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
			Walk(Walk_Down_Left);
		else if(Input.GetKey(KeyCode.W))
			Walk(Walk_Up);
		else if(Input.GetKey(KeyCode.A))
			Walk(Walk_Left);
		else if(Input.GetKey(KeyCode.D))
			Walk(Walk_Right);
		else if(Input.GetKey(KeyCode.S))
			Walk(Walk_Down);
		//Idle
		if(Input.GetKeyUp(KeyCode.W) && Input.GetKeyUp(KeyCode.A))
			player.sprite = Walk_Up_Left[0];
		else if(Input.GetKeyUp(KeyCode.W)&& Input.GetKeyUp(KeyCode.D))
			player.sprite = Walk_Up_Right[0];
		else if(Input.GetKeyUp(KeyCode.S) && Input.GetKeyUp(KeyCode.A))
			player.sprite = Walk_Down_Left[0];
		else if(Input.GetKeyUp(KeyCode.S) && Input.GetKeyUp(KeyCode.D))
			player.sprite = Walk_Down_Right[0];
		else if(Input.GetKeyUp(KeyCode.W))
			player.sprite = Walk_Up[0];
		else if(Input.GetKeyUp(KeyCode.A))
			player.sprite = Walk_Left[0];
		else if(Input.GetKeyUp(KeyCode.D))
			player.sprite = Walk_Right[0];
		else if(Input.GetKeyUp(KeyCode.S))
			player.sprite = Walk_Down[0];



	}

	void Walk(Sprite[] check)
	{
		if (frame <= Walk_Up_Left.Length-1)
		{
			time ++;
			if (time >= Anim_speed)
			{
				player.sprite = check[frame++];
				time = 0;
			}
		} else frame = 0;
	}
}
