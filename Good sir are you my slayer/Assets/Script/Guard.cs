using UnityEngine;
using System.Collections;
using Database;
using UnityEngine.Networking;

public class Guard : NetworkBehaviour {

	public Player Target;
    public Vector3 StartPoint;
    public Unit Unit;
    public LayerMask layermask;

	void Update () 
	{
        if (Target != null)
        {
            Target.IsWanted = true;
            bool walkable = (Physics.CheckSphere(Target.transform.position, 0.5f, layermask));
            if (walkable)
                Unit.MoveTo(Target.transform.position);
            float distance = Vector3.Distance(Target.transform.position, transform.position);
            if (distance < 1.5f)
            {
                Target.Health = 0;
                Unit.MoveTo(StartPoint);
                Target = null;
            }
        }    
    }
}
