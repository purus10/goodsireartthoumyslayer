using UnityEngine;
using System.Collections;
using Database;

public class Player_Animations : MonoBehaviour {
	
	SpriteRenderer player;
	public float Anim_speed;
	float time;
	int frame = 0;
	public Sprite[] Walk_Up;
	public Sprite[] Walk_Up_Left;
	public Sprite[] Walk_Up_Right;
	public Sprite[] Walk_Down;
	public Sprite[] Walk_Down_Left;
	public Sprite[] Walk_Down_Right;
	public Sprite[] Walk_Left;
	public Sprite[] Walk_Right;
	Sprite[] check;
	NetworkView NView;

	void Start()
	{
	player = Get.player.GetComponent<SpriteRenderer>();
	NView  = GetComponent<NetworkView>();
	}
	void Update () 
	{
		if(!NView.isMine) return;
		//walk
		if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
		{
			NView.RPC("AssignCheck",RPCMode.All,1);
			NView.RPC("Walk",RPCMode.All);
		} else if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
		{
			NView.RPC("AssignCheck",RPCMode.All,2);
			NView.RPC("Walk",RPCMode.All);
		} else if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
		{
			NView.RPC("AssignCheck",RPCMode.All,3);
			NView.RPC("Walk",RPCMode.All);
		} else if(Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
		{
			NView.RPC("AssignCheck",RPCMode.All,4);
			NView.RPC("Walk",RPCMode.All);
		} else if(Input.GetKey(KeyCode.W))
		{
			NView.RPC("AssignCheck",RPCMode.All,5);
			NView.RPC("Walk",RPCMode.All);
		} else if(Input.GetKey(KeyCode.A))
		{
			NView.RPC("AssignCheck",RPCMode.All,6);
			NView.RPC("Walk",RPCMode.All);
		} else if(Input.GetKey(KeyCode.D))
		{
			NView.RPC("AssignCheck",RPCMode.All,7);
			NView.RPC("Walk",RPCMode.All);
		} else if(Input.GetKey(KeyCode.S))
			NView.RPC("AssignCheck",RPCMode.All,8);
			NView.RPC("Walk",RPCMode.All);

		//Idle
		if(Input.GetKeyUp(KeyCode.W) && Input.GetKeyUp(KeyCode.A))
			NView.RPC("AssignIdle",RPCMode.All,1);
		else if(Input.GetKeyUp(KeyCode.W)&& Input.GetKeyUp(KeyCode.D))
			NView.RPC("AssignIdle",RPCMode.All,2);
		else if(Input.GetKeyUp(KeyCode.S) && Input.GetKeyUp(KeyCode.D))
			NView.RPC("AssignIdle",RPCMode.All,3);
		else if(Input.GetKeyUp(KeyCode.S) && Input.GetKeyUp(KeyCode.A))
			NView.RPC("AssignIdle",RPCMode.All,4);
		else if(Input.GetKeyUp(KeyCode.W))
			NView.RPC("AssignIdle",RPCMode.All,5);
		else if(Input.GetKeyUp(KeyCode.A))
			NView.RPC("AssignIdle",RPCMode.All,6);
		else if(Input.GetKeyUp(KeyCode.D))
			NView.RPC("AssignIdle",RPCMode.All,7);
		else if(Input.GetKeyUp(KeyCode.S))
			NView.RPC("AssignIdle",RPCMode.All,8);
	}
	[RPC]
	private void AssignIdle(int i)
	{
		if (i == 1) 
			player.sprite = Walk_Up_Left[3];
		else if (i == 2)
			player.sprite = Walk_Up_Right[3];
		else if (i == 3)
			player.sprite = Walk_Down_Right[3];
		else if (i == 4)
			player.sprite = Walk_Down_Left[3];
		else if (i == 5)
			player.sprite = Walk_Up[3];
		else if (i == 6)
			player.sprite = Walk_Left[3];
		else if (i == 7)
			player.sprite = Walk_Right[3];
		else if (i == 8)
			player.sprite = Walk_Down[3];
	}
	[RPC]
	private void AssignCheck(int i)
	{
		if (i == 1) 
			check = Walk_Up_Left;
		else if (i == 2)
			check = Walk_Up_Right;
		else if (i == 3)
			check = Walk_Down_Right;
		else if (i == 4)
			check = Walk_Down_Left;
		else if (i == 5)
			check = Walk_Up;
		else if (i == 6)
			check = Walk_Left;
		else if (i == 7)
			check = Walk_Right;
		else if (i == 8)
			check = Walk_Down;
	}
	[RPC]
	private void Walk()
	{
		if (frame <= Walk_Up_Left.Length-1)
		{
			time++;
			if (time >= Anim_speed && check != null)
			{
				player.sprite = check[frame++];
				time = 0;
				check = null;
			}
		} else frame = 0;
	}
}
