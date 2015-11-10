using UnityEngine;
using System.Collections;
using Database;

public class Player_Animations : MonoBehaviour {

    SpriteRenderer player;
    public float Anim_speed;
    public SpriteRenderer Sprite_body;
    public int _body, _head;
    float time;
   public  int frame = 0;
   public  Vector3 position;
    Sprite[,] Head;
    Sprite[,] Body;
    #region Implement Arrays
    public Sprite[] Head_Walk_Up1;
    public Sprite[] Head_Walk_Up2;
    public Sprite[] Head_Walk_Up3;
    public Sprite[] Head_Walk_Up4;
    public Sprite[] Head_Walk_Up5;
    public Sprite[] Head_Walk_Up6;
    public Sprite[] Head_Walk_Down1;
    public Sprite[] Head_Walk_Down2;
    public Sprite[] Head_Walk_Down3;
    public Sprite[] Head_Walk_Down4;
    public Sprite[] Head_Walk_Down5;
    public Sprite[] Head_Walk_Down6;
    public Sprite[] Head_Walk_Left1;
    public Sprite[] Head_Walk_Left2;
    public Sprite[] Head_Walk_Left3;
    public Sprite[] Head_Walk_Left4;
    public Sprite[] Head_Walk_Left5;
    public Sprite[] Head_Walk_Left6;
    public Sprite[] Head_Walk_Right1;
    public Sprite[] Head_Walk_Right2;
    public Sprite[] Head_Walk_Right3;
    public Sprite[] Head_Walk_Right4;
    public Sprite[] Head_Walk_Right5;
    public Sprite[] Head_Walk_Right6;
    public Sprite[] Body_Walk_Up1;
    public Sprite[] Body_Walk_Up2;
    public Sprite[] Body_Walk_Up3;
    public Sprite[] Body_Walk_Up4;
    public Sprite[] Body_Walk_Up5;
    public Sprite[] Body_Walk_Up6;
    public Sprite[] Body_Walk_Down1;
    public Sprite[] Body_Walk_Down2;
    public Sprite[] Body_Walk_Down3;
    public Sprite[] Body_Walk_Down4;
    public Sprite[] Body_Walk_Down5;
    public Sprite[] Body_Walk_Down6;
    public Sprite[] Body_Walk_Left1;
    public Sprite[] Body_Walk_Left2;
    public Sprite[] Body_Walk_Left3;
    public Sprite[] Body_Walk_Left4;
    public Sprite[] Body_Walk_Left5;
    public Sprite[] Body_Walk_Left6;
    public Sprite[] Body_Walk_Right1;
    public Sprite[] Body_Walk_Right2;
    public Sprite[] Body_Walk_Right3;
    public Sprite[] Body_Walk_Right4;
    public Sprite[] Body_Walk_Right5;
    public Sprite[] Body_Walk_Right6;
    #endregion
    Sprite[] check;
    NetworkView NView;

    void Awake()
    {
        SetHead();
       SetBody();
        
    }
    void Start()
    {
        player = GetComponent<SpriteRenderer>();
        NView = GetComponent<NetworkView>();
        position = transform.position;
        player.sprite = Head[_head, 0];
       Sprite_body.sprite = Body[_body, 0];
    }
    void Update()
    {
        if (!NView.isMine) return;
        //walk
        if (position.y < transform.position.y)
        {
            Walk(6, 11);
            position = transform.position;
            // NView.RPC("Walk",RPCMode.All,6,11);
        }
        else if (position.x > transform.position.x)
        {
            Walk(18, 23);
            position = transform.position;
            //NView.RPC("Walk",RPCMode.All,18,23);
        }
        else if (position.x < transform.position.x)
        {
            Walk(12, 17);
            position = transform.position;
            //NView.RPC("Walk",RPCMode.All,12,17);
        }
        else if (position.y > transform.position.y)
        {
            Walk(0, 5);
            position = transform.position;
            //NView.RPC("Walk",RPCMode.All,0,5);
        }

        //Idle
        if (Input.GetKeyUp(KeyCode.W))
            player.sprite = Head[_head, 6];
		else if(Input.GetKeyUp(KeyCode.A))
            player.sprite = Head[_head, 18];
        else if(Input.GetKeyUp(KeyCode.D))
            player.sprite = Head[_head, 12];
        else if(Input.GetKeyUp(KeyCode.S))
            player.sprite = Head[_head, 0];
    }
    [RPC]
    private void AssignIdle(int i)
    {
        if (i == 1)
        {
            player.sprite = Head[_head, 0];
            Sprite_body.sprite = Body[_body, 0];
        }
        else if (i == 2)
        {
            player.sprite = Head[_head, 6];
            Sprite_body.sprite = Body[_body, 6];
        }
        else if (i == 3)
        {
            player.sprite = Head[_head, 12];
            Sprite_body.sprite = Body[_body, 12];
        }
        else if (i == 4)
        {
            player.sprite = Head[_head, 18];
            Sprite_body.sprite = Body[_body, 18];
        }
    }
    /*[RPC]
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
			check = Walk_Down;*/

    void SetHead()
    {
        Head = new Sprite[Head_Walk_Down1.Length, 24];
        for (int i = 0; i < Head_Walk_Down1.Length;i++)
        {
            for (int j = 0;j < 24;j++)
            {
                switch(j)
                {
                    case 0:
                        Head[i, j] = Head_Walk_Down1[i];
                        break;
                    case 1:
                        Head[i, j] = Head_Walk_Down2[i];
                        break;
                    case 2:
                        Head[i, j] = Head_Walk_Down3[i];
                        break;
                    case 3:
                        Head[i, j] = Head_Walk_Down4[i];
                        break;
                    case 4:
                        Head[i, j] = Head_Walk_Down5[i];
                        break;
                    case 5:
                        Head[i, j] = Head_Walk_Down6[i];
                        break;
                    case 6:
                        Head[i, j] = Head_Walk_Up1[i];
                        break;
                    case 7:
                        Head[i, j] = Head_Walk_Up2[i];
                        break;
                    case 8:
                        Head[i, j] = Head_Walk_Up3[i];
                        break;
                    case 9:
                        Head[i, j] = Head_Walk_Up4[i];
                        break;
                    case 10:
                        Head[i, j] = Head_Walk_Up5[i];
                        break;
                    case 11:
                        Head[i, j] = Head_Walk_Up6[i];
                        break;
                    case 12:
                        Head[i, j] = Head_Walk_Right1[i];
                        break;
                    case 13:
                        Head[i, j] = Head_Walk_Right2[i];
                        break;
                    case 14:
                        Head[i, j] = Head_Walk_Right3[i];
                        break;
                    case 15:
                        Head[i, j] = Head_Walk_Right4[i];
                        break;
                    case 16:
                        Head[i, j] = Head_Walk_Right5[i];
                        break;
                    case 17:
                        Head[i, j] = Head_Walk_Right6[i];
                        break;
                    case 18:
                        Head[i, j] = Head_Walk_Left1[i];
                        break;
                    case 19:
                        Head[i, j] = Head_Walk_Left2[i];
                        break;
                    case 20:
                        Head[i, j] = Head_Walk_Left3[i];
                        break;
                    case 21:
                        Head[i, j] = Head_Walk_Left4[i];
                        break;
                    case 22:
                        Head[i, j] = Head_Walk_Left5[i];
                        break;
                    case 23:
                        Head[i, j] = Head_Walk_Left6[i];
                        break;
                    default:
                        print("ERROR " + j + " NOT ASSIGNED");
                        break;
                }
            }

        }
    }
    void SetBody()
    {
        Body = new Sprite[Body_Walk_Down1.Length, 24];
        for (int i = 0; i < Body.GetLength(0); i++)
        {
            for (int j = 0; j < Body.GetLength(1); j++)
            {
                switch (j)
                {
                    case 0:
                        Body[i, j] = Body_Walk_Down1[i];
                        break;
                    case 1:
                        Body[i, j] = Body_Walk_Down2[i];
                        break;
                    case 2:
                        Body[i, j] = Body_Walk_Down3[i];
                        break;
                    case 3:
                        Body[i, j] = Body_Walk_Down4[i];
                        break;
                    case 4:
                        Body[i, j] = Body_Walk_Down5[i];
                        break;
                    case 5:
                        Body[i, j] = Body_Walk_Down6[i];
                        break;
                    case 6:
                        Body[i, j] = Body_Walk_Up1[i];
                        break;
                    case 7:
                        Body[i, j] = Body_Walk_Up2[i];
                        break;
                    case 8:
                        Body[i, j] = Body_Walk_Up3[i];
                        break;
                    case 9:
                        Body[i, j] = Body_Walk_Up4[i];
                        break;
                    case 10:
                        Body[i, j] = Body_Walk_Up5[i];
                        break;
                    case 11:
                        Body[i, j] = Body_Walk_Up6[i];
                        break;
                    case 12:
                        Body[i, j] = Body_Walk_Down1[i];
                        break;
                    case 13:
                        Body[i, j] = Body_Walk_Down2[i];
                        break;
                    case 14:
                        Body[i, j] = Body_Walk_Down3[i];
                        break;
                    case 15:
                        Body[i, j] = Body_Walk_Down4[i];
                        break;
                    case 16:
                        Body[i, j] = Body_Walk_Down5[i];
                        break;
                    case 17:
                        Body[i, j] = Body_Walk_Down6[i];
                        break;
                    case 18:
                        Body[i, j] = Body_Walk_Up1[i];
                        break;
                    case 19:
                        Body[i, j] = Body_Walk_Up2[i];
                        break;
                    case 20:
                        Body[i, j] = Body_Walk_Up3[i];
                        break;
                    case 21:
                        Body[i, j] = Body_Walk_Up4[i];
                        break;
                    case 22:
                        Body[i, j] = Body_Walk_Up5[i];
                        break;
                    case 23:
                        Body[i, j] = Body_Walk_Up6[i];
                        break;
                    default:
                        print("ERROR " + j + " NOT ASSIGNED");
                        break;
                }
            }

        }
    }
	[RPC]
	private void Walk(int min, int max)
	{
        if (frame < max)
        {
            time++;
            if (time >= Anim_speed)
            {
                frame++;
                player.sprite = Head[_head, frame];
                Sprite_body.sprite = Body[_body, frame];
                time = 0;
            }
        }
        else frame = min;
    }
}
