using UnityEngine;
using System.Collections;

public class Chatter : MonoBehaviour {


    public AudioClip[] Talking;
    public AudioClip Drinking;
    public AudioClip Bathroom;
    public AudioClip Eating; 

    public float nextChat;

    IEnumerator Start()
    {
        Npc Guest = GetComponent<Npc>();
        while (true)
        {
            if (Guest.State == Npc.states.Talking)
            {
                PlaySound(Talking[Random.Range(0, Talking.Length)], 0.1f);
                yield return new WaitForSeconds(nextChat);
            }
            else if (Guest.State == Npc.states.Drink)
            {
                PlaySound(Drinking, 0.1f);
                yield return new WaitForSeconds(nextChat);
            }
            else if (Guest.State == Npc.states.Bathroom)
            {
                PlaySound(Bathroom, 0.1f);
                yield return new WaitForSeconds(nextChat);
            }
            else if (Guest.State == Npc.states.Hungry)
            {
                PlaySound(Eating, 0.1f);
                yield return new WaitForSeconds(nextChat);
            }
            else
            {
                yield return 0;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void PlaySound(AudioClip a, float vol)
    {
        GetComponent<AudioSource>().clip = a;
        GetComponent<AudioSource>().volume = vol;
        GetComponent<AudioSource>().Play();
    }
}
