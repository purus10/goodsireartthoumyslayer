using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GUI_Start : NetworkBehaviour {

    public Rect StartButton;
    static public int clickCount;
    static public bool Start = true;
    public Unit_Spawner Uspawner;


    public override void OnStartServer()
    {
        if (isServer)
        {
            Uspawner.SpawnUnits();
        }
    }


    void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            if (isServer)
            {
                RpcStartMatch();
            }
        }
    }

    [ClientRpc]
    void RpcStartMatch()
    {
            Uspawner.startscreen.SetActive(false);
            Uspawner.startscreen.GetComponent<Camera>().enabled = false;
            Start = false;
    }
}
