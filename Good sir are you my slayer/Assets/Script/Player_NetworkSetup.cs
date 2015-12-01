using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour {
    // Use this for initialization

	void FixedUpdate ()
    {
        if (isLocalPlayer)
        {
            if (GUI_Start.Start == false)
            {
                GetComponent<Player>().enabled = true;
                GetComponent<Player>().GetComponentInChildren<AudioListener>().enabled = true;
            }
            else
            {
                print("REGISTERED");
                GetComponent<Player>().enabled = false;
                GetComponent<Player>().GetComponentInChildren<AudioListener>().enabled = false;
            }
        }
    }
}
