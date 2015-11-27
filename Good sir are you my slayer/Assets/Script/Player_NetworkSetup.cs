using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour {
	// Use this for initialization
	void Start ()
    {
	    if (isLocalPlayer)
        {
            GetComponent<Player>().enabled = true;
        }
	}
}
