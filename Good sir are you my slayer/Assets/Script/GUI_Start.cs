using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class GUI_Start : NetworkBehaviour {

    public Rect StartButton;
    bool starttimer = true;
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
                if (Start == true)
                {
                        Digit.StartTimer = true;
                    Uspawner.RpcDesignateTarget();
                    RpcStartMatch();
                }
               
            }
        }
    }

    [ClientRpc]
    void RpcStartMatch()
    {
            Uspawner.startscreen.SetActive(false);
            //Uspawner.startscreen.GetComponent<Camera>().enabled = false;
            Start = false;
    }
}
