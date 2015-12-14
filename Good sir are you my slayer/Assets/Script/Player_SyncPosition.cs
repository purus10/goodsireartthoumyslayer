using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_SyncPosition : NetworkBehaviour {

    [SyncVar]
    private Vector3 syncPos;

    [SerializeField] Transform myTransform;
    [SerializeField] float LerpRate = 50;

    private Vector3 Lastpos;
    private float threshold = 0.5f;

    void Update()
    {
        LerpPosition();
    }

    void FixedUpdate()
    {
        TransmitPosition();
    }

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            myTransform.position = Vector2.Lerp(myTransform.position, syncPos, Time.deltaTime * LerpRate);
        }
    }

    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(myTransform.position,Lastpos) > threshold)
        {
            CmdProvidePositionToServer(myTransform.position);
            Lastpos = myTransform.position;
        }
    }
}
