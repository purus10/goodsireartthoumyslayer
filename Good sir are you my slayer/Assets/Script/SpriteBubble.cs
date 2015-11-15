using UnityEngine;
using System.Collections;

public class SpriteBubble : MonoBehaviour
{
    public SpriteRenderer Bubble;
    public enum states {None, Talking};
    public states Type;
    public Sprite[] Bubbles;
    public float Anim_speed;
    int frame;
    float time;
	
	void Update ()
    {
        switch (Type)
        {
            case states.Talking:
                {

                    break;
                }
        }
	
	}

    [RPC]
    private void Anim_Bubble(int min, int max)
    {
        if (frame < max)
        {
            time++;
            if (time >= Anim_speed)
            {
                frame++;
                Bubble.sprite = Bubbles[frame];
                time = 0;
            }
        }
        else frame = min;
    }
}
