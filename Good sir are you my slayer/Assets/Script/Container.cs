using UnityEngine;
using System.Collections;

public class Container : MonoBehaviour {

    public GameObject Item1;
    public GameObject Item2;
    public GameObject Item3;
    bool gui;

    void OnTriggerStay(Collider col)
    {
        Player player = col.GetComponent<Player>();
        if (player != null)
        {
            if (Input.GetButtonDown("X"))
            {
                player.SelectedContain = this;
                gui = true;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        gui = false;
    }


	// Use this for initialization
	void OnGUI ()
    {
	    if (gui)
        {
            GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 100f, 25f), Item1.ToString());
            GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 25f, 100f, 25f), Item2.ToString());
            GUI.Button(new Rect(Screen.width / 2, Screen.height / 2 + 50f, 100f, 25f), Item3.ToString());
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
