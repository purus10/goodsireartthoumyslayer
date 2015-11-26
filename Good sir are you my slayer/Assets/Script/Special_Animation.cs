using UnityEngine;
using System.Collections;

public class Special_Animation : MonoBehaviour {

    SpriteRenderer player;
    Guard guard;
    Butler butler;
    public float Anim_speed;
    public int _head;
    float time;
    public int frame;
    public Vector3 position;
    public Sprite[] Head;
    Sprite[] check;

    public void AssignParts()
    {
        player.sprite = Head[6];
    }
    void Start()
    {
        guard = GetComponent<Guard>();
        butler = GetComponent<Butler>();
        player = GetComponent<SpriteRenderer>();
        position = transform.position;
        AssignParts();
    }
    void Update()
    {
        if (guard != null)
        {
            if (guard.Unit.path.Length != 0)
            {
                if (guard.Target == null && transform.position == guard.Unit.path[guard.Unit.path.Length - 1])
                    player.sprite = Head[6];
            }
            else
                player.sprite = Head[6];
        }
        /*if (butler != null && transform.position == butler.Unit.path[butler.Unit.path.Length - 1])
            player.sprite = Head[6];*/
        //walk
        if (position.x > transform.position.x)
        {
            Walk(12, 17);
            position = transform.position;
        }
        else if (position.x < transform.position.x)
        {
            Walk(18, 23);
            position = transform.position;
        }
        else if (position.y < transform.position.y)
        {
            Walk(0, 5);
            position = transform.position;
        }
        else if (position.y > transform.position.y)
        {
            Walk(6, 11);
            position = transform.position;
        }
        position = transform.position;
    }
    private void AssignIdle(int i)
    {
        if (i == 1)
        {
            player.sprite = Head[0];
        }
        else if (i == 2)
        {
            player.sprite = Head[6];
        }
        else if (i == 3)
        {
            player.sprite = Head[12];
        }
        else if (i == 4)
        {
            player.sprite = Head[18];
        }
    }

    public void Walk(int min, int max)
    {
        if (frame < max && frame >= min)
        {
            time++;
            if (time >= Anim_speed)
            {
                frame++;
                player.sprite = Head[frame];
                time = 0;
            }
        }
        else frame = min;
    }
}
