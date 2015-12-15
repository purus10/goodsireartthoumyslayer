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
               // GetComponent<Player>().GetComponentInChildren<AudioListener>().enabled = true;
                GetComponent<CharacterController>().enabled = true;
            }
            else
            {
                GetComponent<Player>().enabled = false;
                //GetComponent<Player>().GetComponentInChildren<AudioListener>().enabled = false;
                GetComponent<CharacterController>().enabled = false;
            }
        }
    }
}
