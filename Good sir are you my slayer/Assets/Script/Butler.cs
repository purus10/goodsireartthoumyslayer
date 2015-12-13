using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Butler : NetworkBehaviour {

    public Vector3[] EndPoint;
    public Unit Unit;
    public LayerMask layermask;

    void Start()
    {
        Unit.MoveTo(EndPoint[Random.Range(0, EndPoint.Length - 1)]);
    }

    void Update()
    {
        if (GUI_Start.Start)
            return;

        if (!isServer)
            return;

        if (Unit.path.Length != 0)
        {
            if (transform.position == Unit.path[Unit.path.Length - 1])
            {
                Unit.MoveTo(EndPoint[Random.Range(0, EndPoint.Length - 1)]);
            }
        } else
            Unit.MoveTo(EndPoint[Random.Range(0, EndPoint.Length - 1)]);
    }
}
