using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SpriteBubble : NetworkBehaviour
{
    public SpriteRenderer Bubble;
    public enum states {None, Talking};
    public Sprite[] Bubbles;
    public float Anim_speed;
    public Npc Npc;
    public Player player;
    int frame;
    float time;
	
	void Update ()
    {
        if (Npc != null)
        {
            switch (Npc.State)
            {
                case Npc.states.Talking:
                    {
                        Anim_Bubble(0, 5);
                        break;
                    }
                case Npc.states.Afraid:
                    {
                        Anim_Bubble(12, 15);
                        break;
                    }
                case Npc.states.Idle:
                    {
                        Bubble.sprite = Bubbles[20];
                        break;
                    }
            }
        } else
        {
            Bubble.sprite = Bubbles[20];
        }
        if (player != null)
        {
            if (player.State == Player.states.Talking)
            {
                Anim_Bubble(0, 5);
            }
            else
            {
                Bubble.sprite = Bubbles[20];
            }
        }
	
	}

    public void Anim_Bubble(int min, int max)
    {
        if (frame < max && frame >= min)
        {
            time++;
            if (time >= Anim_speed)
            {
                frame++;
                Bubble.sprite =Bubbles[frame];
                time = 0;
            }
        }
        else frame = min;
    }
}
