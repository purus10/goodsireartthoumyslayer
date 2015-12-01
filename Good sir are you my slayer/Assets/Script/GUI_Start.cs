using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GUI_Start : NetworkBehaviour {

    public Rect StartButton;
    static public int clickCount;
    static public bool Start = true;
    public Unit_Spawner Uspawner;


    void Update()
    {
        if (Start)
        {
            if (Input.GetButtonDown("Start"))
            {
                clickCount++;
            }
        }

        if (clickCount == Network.connections.Length + 1)
        {
            Uspawner.startscreen.SetActive(false);
            Uspawner.startscreen.GetComponent<Camera>().enabled = false;
            Unit_Spawner.StartMatch = true;
            Start = false;
            clickCount = 0;
        }
    }
}
